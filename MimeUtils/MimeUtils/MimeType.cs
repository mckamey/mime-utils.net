#region License
/*---------------------------------------------------------------------------------*\

	Distributed under the terms of an MIT-style license:

	The MIT License

	Copyright (c) 2006-2009 Stephen M. McKamey

	Permission is hereby granted, free of charge, to any person obtaining a copy
	of this software and associated documentation files (the "Software"), to deal
	in the Software without restriction, including without limitation the rights
	to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
	copies of the Software, and to permit persons to whom the Software is
	furnished to do so, subject to the following conditions:

	The above copyright notice and this permission notice shall be included in
	all copies or substantial portions of the Software.

	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
	IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
	FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
	AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
	LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
	OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
	THE SOFTWARE.

\*---------------------------------------------------------------------------------*/
#endregion License

using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace MimeUtils
{
	/// <summary>
	/// Multipurpose Internet Mail Extensions (MIME) type
	/// </summary>
	[Serializable]
	public class MimeType : IComparable
	{
		#region Constants

		public static readonly MimeType Empty = new MimeType();

		#endregion Constants

		#region Fields

		private string name;
		private string description;
		private string[] fileExts;
		private string[] contentTypes;
		private MimeCategory category;
		private bool primary;

		#endregion Fields

		#region Properties

		[DefaultValue("")]
		[XmlElement("Name")]
		public string Name
		{
			get
			{
				if (this.name == null)
				{
					return String.Empty;
				}
				return this.name;
			}
			set { this.name = value; }
		}

		[DefaultValue("")]
		[XmlElement("Description")]
		public string Description
		{
			get
			{
				if (this.description == null)
				{
					return String.Empty;
				}
				return this.description;
			}
			set { this.description = value; }
		}

		[DefaultValue(null)]
		[XmlElement("FileExt")]
		public string[] FileExts
		{
			get { return this.fileExts; }
			set
			{
				if (value == null)
				{
					value = new string[0];
				}

				foreach (string fileExt in value)
				{
					if (fileExt == null || fileExt.IndexOf('.') < 0)
					{
						throw new FormatException("FileExt is not correct format: "+fileExt);
					}
				}

				this.fileExts = value;
			}
		}

		[DefaultValue(null)]
		[XmlElement("ContentType")]
		public string[] ContentTypes
		{
			get { return this.contentTypes; }
			set
			{
				if (value == null)
				{
					value = new string[0];
				}

				foreach (string contentType in value)
				{
					if (contentType == null || contentType.IndexOf('/') < 0)
					{
						throw new FormatException("ContentType is not correct format: "+contentType);
					}
				}

				this.contentTypes = value;
			}
		}

		[DefaultValue(MimeCategory.Unknown)]
		[XmlElement("Category")]
		public MimeCategory Category
		{
			get { return this.category; }
			set { this.category = value; }
		}

		[Description("Gets and sets this as the primary type when resolution is ambiguous.")]
		[DefaultValue(false)]
		[XmlAttribute("primary")]
		public bool Primary
		{
			get { return this.primary; }
			set { this.primary = value; }
		}

		/// <summary>
		/// Gets the dominant content type for this MIME type.
		/// </summary>
		[XmlIgnore]
		public string ContentType
		{
			get
			{
				if (this.ContentTypes == null || this.ContentTypes.Length < 1)
				{
					return String.Empty;
				}
				return this.ContentTypes[0];
			}
		}

		/// <summary>
		/// Gets the dominant file extension for this MIME type.
		/// </summary>
		[XmlIgnore]
		public string FileExt
		{
			get
			{
				if (this.FileExts == null || this.FileExts.Length < 1)
				{
					return String.Empty;
				}
				return this.FileExts[0];
			}
		}

		#endregion Properties

		#region IComparable Members

		int IComparable.CompareTo(object obj)
		{
			MimeType that = obj as MimeType;
			if (that == null)
			{
				return 1;
			}

			if (this.FileExts.Length == 0 &&
				that.FileExts.Length == 0)
			{
				return this.Name.CompareTo(that.Name);
			}

			return this.FileExts[0].CompareTo(that.FileExts[0]);
		}

		#endregion IComparable Members
	}
}
