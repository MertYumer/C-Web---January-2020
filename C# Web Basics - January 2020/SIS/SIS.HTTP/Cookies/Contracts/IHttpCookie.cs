namespace SIS.HTTP.Cookies.Contracts
{
	using System;
					
	public interface IHttpCookie
	{
		string Key { get; }
		
		string Value { get; }
		
		DateTime Expires { get; }
		
		string Path { get; }
		
		bool IsNew { get; }
		
		bool HttpOnly { get; }
		
		void Delete();
	}
}