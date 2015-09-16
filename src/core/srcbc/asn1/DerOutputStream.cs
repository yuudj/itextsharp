/*
 * $Id$
 *
 * This file is part of the iText (R) project.
 * Copyright (c) 1998-2015 iText Group NV
 * Authors: Bruno Lowagie, Paulo Soares, et al.
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License version 3
 * as published by the Free Software Foundation with the addition of the
 * following permission added to Section 15 as permitted in Section 7(a):
 * FOR ANY PART OF THE COVERED WORK IN WHICH THE COPYRIGHT IS OWNED BY
 * ITEXT GROUP. ITEXT GROUP DISCLAIMS THE WARRANTY OF NON INFRINGEMENT
 * OF THIRD PARTY RIGHTS
 *
 * This program is distributed in the hope that it will be useful, but
 * WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
 * or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU Affero General Public License for more details.
 * You should have received a copy of the GNU Affero General Public License
 * along with this program; if not, see http://www.gnu.org/licenses or write to
 * the Free Software Foundation, Inc., 51 Franklin Street, Fifth Floor,
 * Boston, MA, 02110-1301 USA, or download the license from the following URL:
 * http://itextpdf.com/terms-of-use/
 *
 * The interactive user interfaces in modified source and object code versions
 * of this program must display Appropriate Legal Notices, as required under
 * Section 5 of the GNU Affero General Public License.
 *
 * In accordance with Section 7(b) of the GNU Affero General Public License,
 * a covered work must retain the producer line in every PDF that is created
 * or manipulated using iText.
 *
 * You can be released from the requirements of the license by purchasing
 * a commercial license. Buying such a license is mandatory as soon as you
 * develop commercial activities involving the iText software without
 * disclosing the source code of your own applications.
 * These activities include: offering paid services to customers as an ASP,
 * serving PDFs on the fly in a web application, shipping iText with a closed
 * source product.
 *
 * For more information, please contact iText Software Corp. at this
 * address: sales@itextpdf.com
 */

using System;
using System.IO;

using Org.BouncyCastle.Asn1.Utilities;

namespace Org.BouncyCastle.Asn1
{
	public class DerOutputStream
		: FilterStream
	{
		public DerOutputStream(Stream os)
			: base(os)
		{
		}

		private void WriteLength(
			int length)
		{
			if (length > 127)
			{
				int size = 1;
				uint val = (uint) length;

				while ((val >>= 8) != 0)
				{
					size++;
				}

				WriteByte((byte)(size | 0x80));

				for (int i = (size - 1) * 8; i >= 0; i -= 8)
				{
					WriteByte((byte)(length >> i));
				}
			}
			else
			{
				WriteByte((byte)length);
			}
		}

		internal void WriteEncoded(
			int		tag,
			byte[]	bytes)
		{
			WriteByte((byte) tag);
			WriteLength(bytes.Length);
			Write(bytes, 0, bytes.Length);
		}

		internal void WriteEncoded(
			int		tag,
			byte[]	bytes,
			int		offset,
			int		length)
		{
			WriteByte((byte) tag);
			WriteLength(length);
			Write(bytes, offset, length);
		}

		internal void WriteTag(
			int	flags,
			int	tagNo)
		{
			if (tagNo < 31)
			{
				WriteByte((byte)(flags | tagNo));
			}
			else
			{
				WriteByte((byte)(flags | 0x1f));
				if (tagNo < 128)
				{
					WriteByte((byte)tagNo);
				}
				else
				{
					byte[] stack = new byte[5];
					int pos = stack.Length;

					stack[--pos] = (byte)(tagNo & 0x7F);

					do
					{
						tagNo >>= 7;
						stack[--pos] = (byte)(tagNo & 0x7F | 0x80);
					}
					while (tagNo > 127);

					Write(stack, pos, stack.Length - pos);
				}
			}
		}

		internal void WriteEncoded(
			int		flags,
			int		tagNo,
			byte[]	bytes)
		{
			WriteTag(flags, tagNo);
			WriteLength(bytes.Length);
			Write(bytes, 0, bytes.Length);
		}

		protected void WriteNull()
		{
			WriteByte(Asn1Tags.Null);
			WriteByte(0x00);
		}

		[Obsolete("Use version taking an Asn1Encodable arg instead")]
		public virtual void WriteObject(
			object obj)
		{
			if (obj == null)
			{
				WriteNull();
			}
			else if (obj is Asn1Object)
			{
				((Asn1Object)obj).Encode(this);
			}
			else if (obj is Asn1Encodable)
			{
				((Asn1Encodable)obj).ToAsn1Object().Encode(this);
			}
			else
			{
				throw new IOException("object not Asn1Object");
			}
		}

		public virtual void WriteObject(
			Asn1Encodable obj)
		{
			if (obj == null)
			{
				WriteNull();
			}
			else
			{
				obj.ToAsn1Object().Encode(this);
			}
		}

		public virtual void WriteObject(
			Asn1Object obj)
		{
			if (obj == null)
			{
				WriteNull();
			}
			else
			{
				obj.Encode(this);
			}
		}
	}
}
