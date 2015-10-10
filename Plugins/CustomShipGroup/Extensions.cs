using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CustomShipGroup
{
	static class Extensions
	{
		static MethodInfo addMethod = null;
		static MethodInfo removeMethod = null;

		public static void Add<TData>( this IDDictionary<TData> dict, TData data ) where TData : class, ElectronicObserver.Data.IIdentifiable
		{
			if ( dict == null )
				return;

			if ( addMethod == null )
				addMethod = dict.GetType().GetMethod( "Add", BindingFlags.NonPublic | BindingFlags.Instance );

			//if ( addMethod != null )
				addMethod.Invoke( dict, new object[] { data } );
		}

		public static void Remove<TData>( this IDDictionary<TData> dict, TData data ) where TData : class, ElectronicObserver.Data.IIdentifiable
		{
			if ( dict == null )
				return;

			if ( removeMethod == null ) {
				removeMethod = dict.GetType().GetMethods( BindingFlags.NonPublic | BindingFlags.Instance ).First( m => m.Name == "Remove" && m.IsGenericMethod );
			}

			//if ( removeMethod != null )
				removeMethod.Invoke( dict, new object[] { data } );
		}

	}
}
