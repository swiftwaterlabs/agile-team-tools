﻿using System.IO;

namespace AgileTeamTools.Api.Test.Testing.Extensions
{
    public static class StringExtensions
    {
        public static Stream ToStream(this string value)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            writer.Write(value);
            writer.Flush();
            stream.Position = 0;

            return stream;
        }
    }
}
