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


		public ExpressionManager() {
			Expressions = new List<ExpressionList>();
			predicate = null;
			expression = null;
		}


		public ExpressionList this[int index] {
			get { return Expressions[index]; }
			set { Expressions[index] = value; }
		}


		public void Compile() {
			Expression ex = null;
			var paramex = Expression.Parameter( typeof( ShipData ) );

			foreach ( var exlist in Expressions ) {
				if ( !exlist.Enabled )
					continue;

				if ( ex == null ) {
					ex = exlist.Compile( paramex );

				} else {
					if ( exlist.ExternalAnd ) {
						ex = Expression.AndAlso( ex, exlist.Compile( paramex ) );
					} else {
						ex = Expression.OrElse( ex, exlist.Compile( paramex ) );
					}
				}
			}


			if ( ex == null ) {
				ex = Expression.Constant( true, typeof( bool ) );		//:-P
			}

			predicate = Expression.Lambda<Func<ShipData, bool>>( ex, paramex );
			expression = ex;

		}


		public IEnumerable<ShipData> GetResult( IEnumerable<ShipData> list ) {

			if ( predicate == null )
				throw new InvalidOperationException( "式がコンパイルされていません。" );

			return list.AsQueryable().Where( predicate ).AsEnumerable();
		}

		public bool IsAvailable { get { return predicate != null; } }



		public override string ToString() {
			return expression.ToString();
		}



		public ExpressionManager Clone() {
			var clone = (ExpressionManager)MemberwiseClone();
			clone.Expressions = Expressions == null ? null : new List<ExpressionList>( Expressions );
			return clone;
		}

		object ICloneable.Clone() {
			return Clone();
		}
	}

}
