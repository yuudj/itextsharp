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

using Org.BouncyCastle.Asn1.X509;

namespace Org.BouncyCastle.Asn1.Mozilla
{
	/**
	 * This is designed to parse
	 * the PublicKeyAndChallenge created by the KEYGEN tag included by
	 * Mozilla based browsers.
	 *  <pre>
	 *  PublicKeyAndChallenge ::= SEQUENCE {
	 *    spki SubjectPublicKeyInfo,
	 *    challenge IA5STRING
	 *  }
	 *
	 *  </pre>
	 */
	public class PublicKeyAndChallenge
		: Asn1Encodable
	{
		private Asn1Sequence			pkacSeq;
		private SubjectPublicKeyInfo	spki;
		private DerIA5String			challenge;

		public static PublicKeyAndChallenge GetInstance(
			object obj)
		{
			if (obj is PublicKeyAndChallenge)
			{
				return (PublicKeyAndChallenge) obj;
			}

			if (obj is Asn1Sequence)
			{
				return new PublicKeyAndChallenge((Asn1Sequence) obj);
			}

			throw new ArgumentException(
				"unknown object in 'PublicKeyAndChallenge' factory : "
				+ obj.GetType().Name + ".");
		}

		public PublicKeyAndChallenge(
			Asn1Sequence seq)
		{
			pkacSeq = seq;
			spki = SubjectPublicKeyInfo.GetInstance(seq[0]);
			challenge = DerIA5String.GetInstance(seq[1]);
		}

		public override Asn1Object ToAsn1Object()
		{
			return pkacSeq;
		}

		public SubjectPublicKeyInfo SubjectPublicKeyInfo
		{
			get { return spki; }
		}

		public DerIA5String Challenge
		{
			get { return challenge; }
		}
	}
}
