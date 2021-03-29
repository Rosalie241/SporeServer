// dllmain.cpp : Defines the entry point for the DLL application.
#define _CRT_SECURE_NO_WARNINGS
#include "stdafx.h"

void Initialize()
{
	// This method is executed when the game starts, before the user interface is shown
	// Here you can do things such as:
	//  - Add new cheats
	//  - Add new simulator classes
	//  - Add new game modes
	//  - Add new space tools
	//  - Change materials
}

void Dispose()
{
	// This method is called when the game is closing
}

void AttachDetours()
{
	// Call the attach() method on any detours you want to add
	// For example: cViewer_SetRenderType_detour::attach(GetAddress(cViewer, SetRenderType));
}

static void DisplayError(const char* fmt, ...)
{
	char buf[200];

	va_list args;
	va_start(args, fmt);
	vsprintf(buf, fmt, args);
	va_end(args);

	MessageBoxA(NULL, buf, "SporeForceHTTP", MB_OK | MB_ICONERROR);
}

// TODO, get the actual module size at runtime
#define MODULE_SIZE 24885248

static void ForceHTTP()
{
	const char original_str[] = "https://%hs%hs";
	const char replace_str[] = "http://%hs%hs";

	HANDLE handle = GetCurrentProcess();

	LPVOID base_addr = GetModuleHandle(NULL);
	uint32_t addr = (uint32_t)base_addr;

	// attempt to find original_str
	bool found_bytes = false;
	while (addr++ <= ((uint32_t)base_addr + MODULE_SIZE))
	{
		found_bytes = (strcmp((char*)addr, (char*)original_str) == 0);
		if (found_bytes)
			break;
	}

	if (!found_bytes)
	{
		DisplayError("Failed to find required byte sequence");
		return;
	}

	// when we find original_str:
	// 1) set memory permissions to readwrite
	// 2) write to memory
	// 3) restore original permissions

	DWORD old_protect;
	SIZE_T bytes_todo;
	if (!VirtualProtect((LPVOID)addr, sizeof(replace_str), PAGE_EXECUTE_READWRITE, &old_protect))
	{
		DisplayError("VirtualProtect(0x%x) Failed: %li", (uint32_t)addr, GetLastError());
		return;
	}

	if (!WriteProcessMemory(handle, (LPVOID)addr, replace_str, sizeof(replace_str), &bytes_todo))
	{
		DisplayError("WriteProcessMemory(0x%x) Failed: %li", (uint32_t)addr, GetLastError());
		// don't return here, we still need to restore permissions
	}

	if (!VirtualProtect((LPVOID)addr, sizeof(replace_str), old_protect, &old_protect))
	{
		DisplayError("VirtualProtect(0x%x) Failed: %li", (uint32_t)addr, GetLastError());
		return;
	}
}


// Generally, you don't need to touch any code here
BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
					 )
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
		ModAPI::AddPostInitFunction(Initialize);
		ModAPI::AddDisposeFunction(Dispose);

		PrepareDetours(hModule);
		AttachDetours();
		CommitDetours();

		ForceHTTP();
		break;

	case DLL_PROCESS_DETACH:
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
		break;
	}
	return TRUE;
}

