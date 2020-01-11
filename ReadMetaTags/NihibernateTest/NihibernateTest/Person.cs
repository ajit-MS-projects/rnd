using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.ActiveRecord;

namespace NihibernateTest
{
	[ActiveRecord]
	public class Person : ActiveRecordBase<Person>
	{
		private int id;
		private string username;
		private string password;

		public Person() { }

		public Person(string username, string password)
		{
			this.username = username;
			this.password = password;
		}

		[PrimaryKey]
		public int Id
		{
			get { return this.id; }
			set { this.id = value; }
		}

		[Property]
		public string Username
		{
			get { return this.username; }
			set { this.username = value; }
		}

		[Property]
		public string Password
		{
			get { return this.password; }
			set { this.password = value; }
		}
	}
}