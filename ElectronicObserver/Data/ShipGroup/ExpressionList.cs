using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.ShipGroup {

	[DataContract( Name = "ExpressionList" )]
	public class ExpressionList : ICloneable {

		[DataMember]
		public List<ExpressionData> Expressions { get; set; }


		[DataMember]
		public bool InternalAnd { get; set; }

		[IgnoreDataMember]
		public bool InternalOr {
			get { return !InternalAnd; }
			set { InternalAnd = !value; }
		}


		[DataMember]
		public bool ExternalAnd { get; set; }

		[IgnoreDataMember]
		public bool ExternalOr {
			get { return !ExternalAnd; }
			set { ExternalAnd = !value; }
		}


		[DataMember]
		public bool Inverse { get; set; }

		[DataMember]
		public bool Enabled { get; set; }


		public ExpressionList() {
			Expressions = new List<ExpressionData>();
			InternalAnd = true;
			ExternalAnd = true;
			Inverse = false;
			Enabled = true;
		}

		public ExpressionList( bool isInternalAnd, bool isExternalAnd, bool inverse ) {
			Expressions = new List<ExpressionData>();
			InternalAnd = isInternalAnd;
			ExternalAnd = isExternalAnd;
			Inverse = inverse;
			Enabled = true;
		}

		public Expression Compile() {
			Expression ex = null;

			foreach ( var exdata in Expressions ) {
				if ( !exdata.Enabled )
					continue;

				if ( ex == null ) {
					ex = exdata.Compile();

				} else {
					if ( InternalAnd ) {
						ex = Expression.AndAlso( ex, exdata.Compile() );
					} else {
						ex = Expression.OrElse( ex, exdata.Compile() );
					}
				}
			}

			if ( Inverse )
				ex = Expression.Not( ex );

			return ex;
		}


		public override string ToString() {
			return string.Format( "({0}){1}", string.Join( InternalAnd ? " かつ " : " または ", Expressions ), Inverse ? " ではない" : "" );			
		}



		public ExpressionList Clone() {
			var clone = (ExpressionList)MemberwiseClone();
			clone.Expressions = new List<ExpressionData>( Expressions );
			return clone;
		}

		object ICloneable.Clone() {
			return Clone();
		}
	}
}
