﻿// <copyright file="TestImage.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageSharp.Tests
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using ImageSharp.Formats;
    using ImageSharp.PixelFormats;

    /// <summary>
    /// A test image file.
    /// </summary>
    public class TestFile
    {
        /// <summary>
        /// The test file cache.
        /// </summary>
        private static readonly ConcurrentDictionary<string, TestFile> Cache = new ConcurrentDictionary<string, TestFile>();

        /// <summary>
        /// The formats directory, as lazy value
        /// </summary>
        // ReSharper disable once InconsistentNaming
        private static readonly Lazy<string> inputImagesDirectory = new Lazy<string>(() => TestEnvironment.InputImagesDirectoryFullPath);
        
        /// <summary>
        /// The image.
        /// </summary>
        private readonly Image<Rgba32> image;

        /// <summary>
        /// The file.
        /// </summary>
        private readonly string file;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestFile"/> class.
        /// </summary>
        /// <param name="file">The file.</param>
        private TestFile(string file)
        {
            this.file = file;

            this.Bytes = File.ReadAllBytes(file);
            this.image = Image.Load<Rgba32>(this.Bytes);
        }

        /// <summary>
        /// Gets the bytes.
        /// </summary>
        public byte[] Bytes { get; }

        /// <summary>
        /// The file name.
        /// </summary>
        public string FilePath => this.file;

        /// <summary>
        /// The file name.
        /// </summary>
        public string FileName => Path.GetFileName(this.file);

        /// <summary>
        /// The file name without extension.
        /// </summary>
        public string FileNameWithoutExtension => Path.GetFileNameWithoutExtension(this.file);

        /// <summary>
        /// Gets the Input Images directory.
        /// </summary>
        private static string InputImagesDirectory => inputImagesDirectory.Value;
        
        /// <summary>
        /// Gets the full qualified path to the input test file.
        /// </summary>
        /// <param name="file">
        /// The file path.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetInputFileFullPath(string file)
        {
            return Path.Combine(InputImagesDirectory, file);
        }

        /// <summary>
        /// Creates a new test file or returns one from the cache.
        /// </summary>
        /// <param name="file">The file path.</param>
        /// <returns>
        /// The <see cref="TestFile"/>.
        /// </returns>
        public static TestFile Create(string file)
        {
            return Cache.GetOrAdd(file, (string fileName) => new TestFile(GetInputFileFullPath(file)));
        }

        /// <summary>
        /// Gets the file name.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string GetFileName(object value)
        {
            return $"{this.FileNameWithoutExtension}-{value}{Path.GetExtension(this.file)}";
        }

        /// <summary>
        /// Gets the file name without extension.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string GetFileNameWithoutExtension(object value)
        {
            return this.FileNameWithoutExtension + "-" + value;
        }

        /// <summary>
        /// Creates a new image.
        /// </summary>
        /// <returns>
        /// The <see cref="Image"/>.
        /// </returns>
        public Image<Rgba32> CreateImage()
        {
            return new Image<Rgba32>(this.image);
        }

        /// <summary>
        /// Creates a new image.
        /// </summary>
        /// <returns>
        /// The <see cref="Image"/>.
        /// </returns>
        public Image<Rgba32> CreateImage(IImageDecoder decoder)
        {
            return Image.Load(this.image.Configuration, this.Bytes, decoder);
        }
    }
}
