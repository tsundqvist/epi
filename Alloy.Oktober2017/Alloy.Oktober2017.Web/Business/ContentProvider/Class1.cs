using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Alloy.Oktober2017.Web.Business.ContentProvider
{
	using EPiServer.Core;
	using EPiServer.DataAbstraction;

	// C
	public class MyContentStore: ContentStore  //ContentStore
	{
	}
	// This is the thing we save
	public class MyContentBase : ContentBase  //ContentStore
	{
		public MyContentBase()
		{
			var store = new MyContentStore();
			store.ListAll();
		}
	}

	// this can be used to load custom icontent if not stored in our db
	public class MyContentProvider : ContentProvider  //ContentStore
	{
		protected override IContent LoadContent(ContentReference contentLink, ILanguageSelector languageSelector)
		{
			throw new NotImplementedException();
		}
	}
	
}