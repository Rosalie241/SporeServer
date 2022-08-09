/*
 * SporeServer - https://github.com/Rosalie241/SporeServer
 *  Copyright (C) 2021 Rosalie Wanders <rosalie@mailbox.org>
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU Affero General Public License version 3.
 *  You should have received a copy of the GNU Affero General Public License
 *  along with this program. If not, see <https://www.gnu.org/licenses/>.
 */
using System;

namespace SporeServer.Utils
{
    internal class Program
    {
        static void ShowUsage(Utils.IUtil[] utils)
        {
            foreach (var util in utils)
            {
                Console.WriteLine($"-> {util.GetName()}");
                Console.WriteLine("Usage:");
                Console.WriteLine(util.GetUsage());
            }
        }

        static void Main(string[] args)
        {
            Utils.IUtil[] utils =
            {
                new Utils.ExtractArcheTypeInfo(),
                new Utils.ExtractBlockInfo()
            };
            
            if (args.Length == 0)
            {
                ShowUsage(utils);
                return;
            }

            foreach (var util in utils)
            {
                if (util.GetName() == args[0])
                {
                    util.Execute(args.Skip(1).ToArray());
                    return;
                }
            }

            ShowUsage(utils);
        }
    }
}