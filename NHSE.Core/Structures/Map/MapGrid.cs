﻿using System;

namespace NHSE.Core
{
    /// <summary>
    /// Basic logic implementation for interacting with the manipulatable map grid.
    /// </summary>
    public abstract class MapGrid
    {
        public static readonly AcreCoordinate[] Acres = AcreCoordinate.GetGrid(AcreWidth, AcreHeight);

        protected MapGrid(int gw, int gh, int mw, int mh)
        {
            GridWidth = gw;
            GridHeight = gh;
            MaxWidth = mw;
            MaxHeight = mh;
        }

        public readonly int GridWidth;
        public readonly int GridHeight;
        public readonly int MaxWidth;
        public readonly int MaxHeight;

        public const int AcreWidth = 7;
        public const int AcreHeight = 6;
        public const int AcreCount = AcreWidth * AcreHeight;

        public int GridTileCount => GridWidth * GridHeight;
        public int MaxTileCount => MaxWidth * MaxHeight;

        public const int MapTileCount16x16 = 16 * 16 * AcreCount;
        public const int MapTileCount32x32 = 32 * 32 * AcreCount;

        protected int GetTileIndex(int x, int y) => (MaxHeight * x) + y;

        protected int GetTileIndex(int acreX, int acreY, int gridX, int gridY)
        {
            var x = (acreX * GridWidth) + gridX;
            var y = (acreY * GridHeight) + gridY;
            return GetTileIndex(x, y);
        }

        protected int GetAcreTileIndex(int acreIndex, int tileIndex)
        {
            var acre = Acres[acreIndex];
            var x = (tileIndex % GridWidth);
            var y = (tileIndex / GridHeight);
            return GetTileIndex(acre.X, acre.Y, x, y);
        }

        public int GetAcre(int x, int y) => (x / GridWidth) + ((y / GridHeight) * AcreWidth);

        public void GetViewAnchorCoordinates(ref int x, ref int y, bool centerReticle)
        {
            // If we aren't snapping the reticle to the nearest acre
            // we want to put the middle of the reticle rectangle where the cursor is.
            // Adjust the view coordinate
            if (!centerReticle)
            {
                // Reticle size is GridWidth, center = /2
                x -= GridWidth / 2;
                y -= GridWidth / 2;
            }

            // Clamp to viewport dimensions, and center to nearest acre if desired.
            // Clamp to boundaries so that we always have 16x16 to view.
            int maxX = ((AcreWidth - 1) * GridWidth);
            int maxY = ((AcreHeight - 1) * GridHeight);
            x = Math.Max(0, Math.Min(x, maxX));
            y = Math.Max(0, Math.Min(y, maxY));
        }

        public void GetViewAnchorCoordinates(int acre, out int x, out int y)
        {
            x = (acre % AcreWidth) * GridWidth;
            y = (acre / AcreWidth) * GridHeight;
        }

        public void ClampCoordinates(ref int x, ref int y)
        {
            x = Math.Max(0, Math.Min(x, MaxWidth - 1));
            y = Math.Max(0, Math.Min(y, MaxHeight - 1));
        }
    }
}
