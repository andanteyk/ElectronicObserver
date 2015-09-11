using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.ShipGroup {


	[DataContract( Name = "ExpressionManager" )]
	public class ExpressionManager : ICloneable {

		[DataMember]
		public List<ExpressionList> Expressions { get; set; }

		[IgnoreDataMember]
		private Expression<Func<ShipData, bool>> predicate;

		[IgnoreDataMember]
		private Expression expression;


		public void Compile() {
			Expression ex = null;

			foreach ( var exlist in Expressions ) {
				if ( !exlist.Enabled )
					continue;

				if ( ex == null ) {
					ex = exlist.Compile();

				} else {
					if ( exlist.ExternalAnd ) {
						ex = Expression.AndAlso( ex, exlist.Compile() );
					} else {
						ex = Expression.OrElse( ex, exlist.Compile() );
					}
				}
			}

			predicate = Expression.Lambda<Func<ShipData, bool>>( ex, Expression.Parameter( typeof( ShipData ) ) );
			expression = ex;
		}


		public IEnumerable<ShipData> GetResult( IEnumerable<ShipData> list ) {

			if ( predicate == null )
				throw new InvalidOperationException( "式がコンパイルされていません。" );

			return list.AsQueryable().Where( predicate ).AsEnumerable();
		}


		public override string ToString() {
			return expression.ToString();
		}



		public ExpressionManager Clone() {
			var clone = (ExpressionManager)MemberwiseClone();
			clone.Expressions = new List<ExpressionList>( Expressions );
			return clone;
		}

		object ICloneable.Clone() {
			return Clone();
		}
	}

}
