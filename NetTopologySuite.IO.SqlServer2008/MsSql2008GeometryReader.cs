// Copyright 2008 - Ricardo Stuven (rstuven@gmail.com)
//
// This file is part of NetTopologySuite.IO.SqlServer2008
// The original source is part of of NHibernate.Spatial.
// NetTopologySuite.IO.SqlServer2008 is free software; you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// NetTopologySuite.IO.SqlServer2008 is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.

// You should have received a copy of the GNU Lesser General Public License
// along with NetTopologySuite.IO.SqlServer2008 if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA 

using System;
using GeoAPI.Geometries;
using GeoAPI.IO;
using Microsoft.SqlServer.Types;
using System.IO;

namespace NetTopologySuite.IO
{
    /// <summary>
    /// Class to read <see cref="SqlGeography"/> and convert to <see cref="IGeometry"/>
    /// </summary>
    public class MsSql2008GeometryReader : IBinaryGeometryReader, IGeometryReader<SqlGeometry>
    {
        /// <summary>
        /// (Obsolete) Gets or sets a factory to use when building the result geometries.
        /// </summary>
        [Obsolete]
        public IGeometryFactory Factory { get; set; }

        /// <inheritdoc cref="IGeometryReader{TSource}.Read(TSource)"/>>
        public IGeometry Read(byte[] bytes)
        {
            using (var stream = new MemoryStream(bytes))
            {
                return Read(stream);
            }
        }

        /// <inheritdoc cref="IGeometryReader{TSource}.Read(Stream)"/>>
        public IGeometry Read(Stream stream)
        {
            using (var reader = new BinaryReader(stream))
            {
                var sqlGeometry = new SqlGeometry();
                sqlGeometry.Read(reader);
                return Read(sqlGeometry);
            }
        }

        /// <inheritdoc cref="IGeometryReader{TSource}.Read(TSource)"/>>
        public IGeometry Read(SqlGeometry geometry)
        {
            var builder = new NtsGeometrySink(GeoAPI.GeometryServiceProvider.Instance);
            geometry.Populate(builder);
            return builder.ConstructedGeometry;
        }

        /// <inheritdoc cref="IGeometryReader{TSource}.RepairRings"/>>
        public bool RepairRings { get; set; }

        #region Implementation of IGeometryIOSettings

        /// <inheritdoc cref="IGeometryIOSettings.HandleSRID"/>>
        public bool HandleSRID
        {
            get { return true; }
            set { }
        }

        /// <inheritdoc cref="IGeometryIOSettings.AllowedOrdinates"/>
        public Ordinates AllowedOrdinates
        {
            get { return GeoAPI.GeometryServiceProvider.Instance.DefaultCoordinateSequenceFactory.Ordinates & Ordinates.XYZM; }
        }

        private Ordinates _handleOrdinates;

        /// <inheritdoc cref="IGeometryIOSettings.HandleOrdinates"/>>
        public Ordinates HandleOrdinates
        {
            get { return _handleOrdinates; }
            set
            {
                value |= AllowedOrdinates;
                _handleOrdinates = value;
            }
        }

        #endregion
    }
}
