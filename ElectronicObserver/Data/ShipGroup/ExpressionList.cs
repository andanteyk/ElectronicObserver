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
			ExternalOr = true;
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


		public ExpressionData this[int index] {
			get { return Expressions[index]; }
			set { Expressions[index] = value; }
		}


		public Expression Compile( ParameterExpression paramex ) {
			Expression ex = null;

			foreach ( var exdata in Expressions ) {
				if ( !exdata.Enabled )
					continue;

				if ( ex == null ) {
					ex = exdata.Compile( paramex );

				} else {
					if ( InternalAnd ) {
						ex = Expression.AndAlso( ex, exdata.Compile( paramex ) );
					} else {
						ex = Expression.OrElse( ex, exdata.Compile( paramex ) );
					}
				}
			}

			if ( ex == null )
				ex = Expression.Constant( true );

			if ( Inverse )
				ex = Expression.Not( ex );

			return ex;
		}


		public override string ToString() {
			var exp = Expressions.Where( p => p.Enabled );
			return string.Format( "({0}){1}", exp.Count() == 0 ? "なし" : string.Join( InternalAnd ? " かつ " : " または ", exp ), Inverse ? " を満たさない" : "" );
		}



		public ExpressionList Clone() {
			var clone = (ExpressionList)MemberwiseClone();
			clone.Expressions = Expressions == null ? null : Expressions.Select( e => e.Clone() ).ToList();
			return clone;
		}

		object ICloneable.Clone() {
			return Clone();
		}
	}
}
