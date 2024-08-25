using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Helpers
{
	public class PaginatedList<T> : List<T>
	{
		public int PageIndex { get; private set; }
		public int TotalPages { get; private set; }
		public int TotalItems { get; private set; }

		public PaginatedList(IEnumerable<T> items, int count, int pageIndex, int pageSize)
		{
			PageIndex = pageIndex;
			TotalPages = (int)Math.Ceiling(count / (double)pageSize);
			TotalItems = count;
			AddRange(items);
		}

		public bool HasPreviousPage => PageIndex > 1;
		public bool HasNextPage => PageIndex < TotalPages;

		public static PaginatedList<T> Create(IEnumerable<T> source, int pageIndex, int pageSize)
		{
			var count = source.Count();
			var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
			return new PaginatedList<T>(items, count, pageIndex, pageSize);
		}

		public static PaginatedList<T> Create(IEnumerable<T> source, int pageIndex, int pageSize, int totalItemCount)
		{
			var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
			return new PaginatedList<T>(items, totalItemCount, pageIndex, pageSize);
		}
	}
}
