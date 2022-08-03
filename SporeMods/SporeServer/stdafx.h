// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently, but
// are changed infrequently
//

#pragma once

#include "targetver.h"

#define _CRT_SECURE_NO_WARNINGS
#define _WINSOCK_DEPRECATED_NO_WARNINGS
#define WIN32_LEAN_AND_MEAN  // Exclude rarely-used stuff from Windows headers
#define UI TMPUI // fix for openssl UI define

// Windows Header Files:
#include <windows.h>

// This is used everywhere
#include <Spore\BasicIncludes.h>

#undef UI // undefine it cos its no longer needed
