﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibiaCAMDecryptor {
    public static class OtConstants {
        public enum FluidColors : byte {
            FLUID_EMPTY = 0x00,
            FLUID_BLUE = 0x01,
            FLUID_RED = 0x02,
            FLUID_BROWN = 0x03,
            FLUID_GREEN = 0x04,
            FLUID_YELLOW = 0x05,
            FLUID_WHITE = 0x06,
            FLUID_PURPLE = 0x07
        }

        public enum FluidTypes {
            FLUID_NONE = FluidColors.FLUID_EMPTY,
            FLUID_WATER = FluidColors.FLUID_BLUE,
            FLUID_BLOOD = FluidColors.FLUID_RED,
            FLUID_BEER = FluidColors.FLUID_BROWN,
            FLUID_SLIME = FluidColors.FLUID_GREEN,
            FLUID_LEMONADE = FluidColors.FLUID_YELLOW,
            FLUID_MILK = FluidColors.FLUID_WHITE,
            FLUID_MANA = FluidColors.FLUID_PURPLE,

            FLUID_LIFE = FluidColors.FLUID_RED + 8,
            FLUID_OIL = FluidColors.FLUID_BROWN + 8,
            FLUID_URINE = FluidColors.FLUID_YELLOW + 8,
            FLUID_COCONUTMILK = FluidColors.FLUID_WHITE + 8,
            FLUID_WINE = FluidColors.FLUID_PURPLE + 8,

            FLUID_MUD = FluidColors.FLUID_BROWN + 16,
            FLUID_FRUITJUICE = FluidColors.FLUID_YELLOW + 16,

            FLUID_LAVA = FluidColors.FLUID_RED + 24,
            FLUID_RUM = FluidColors.FLUID_BROWN + 24,
            FLUID_SWAMP = FluidColors.FLUID_GREEN + 24,

            FLUID_TEA = FluidColors.FLUID_BROWN + 32,
            FLUID_MEAD = FluidColors.FLUID_BROWN + 40
        };

        public static readonly byte[] ReverseFluidMap =
        {
            (byte)FluidColors.FLUID_EMPTY,
            (byte)FluidTypes.FLUID_WATER,
            (byte)FluidTypes.FLUID_MANA,
            (byte)FluidTypes.FLUID_BEER,
            (byte)FluidColors.FLUID_EMPTY,
            (byte)FluidTypes.FLUID_BLOOD,
            (byte)FluidTypes.FLUID_SLIME,
            (byte)FluidColors.FLUID_EMPTY,
            (byte)FluidTypes.FLUID_LEMONADE,
            (byte)FluidTypes.FLUID_MILK
        };

    }
}
