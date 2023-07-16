using Microsoft.EntityFrameworkCore;
using SocialApp.Application.Models;

namespace SocialApp.Application.Services;

internal class Pager<T>
{
	private int _pageSize;
	private int _pageNumber;

    public int PageSize
	{
		get => _pageSize;
		private set => _pageSize = Math.Clamp(value, _minPageSize, _maxPageSize);
	}
	public int PageNumber 
	{ 
		get => _pageNumber;
		private set => _pageNumber = Math.Max(value, 1);
	}

	private const int _maxPageSize = 20;
	private const int _minPageSize = 5;

	public Pager(int pageSize, int pageNumber)
	{
		PageSize = pageSize;
		PageNumber = pageNumber;
	}

	public async Task<PagedList<T>> ToPagedList(IQueryable<T> query, CancellationToken cancellationToken = default)
	{
		int count = await query.CountAsync(cancellationToken);
		int totalPages = (int) Math.Ceiling(count / (double)PageSize);
		query = ApplyPagination(query);
		var list = await query.ToListAsync(cancellationToken);
		return new PagedList<T>
		{
			Items = list,
			PageNumber = PageNumber,
			PageSize = PageSize,
			TotalCount = count,
			TotalPages = totalPages
		};
	}

	private IQueryable<T> ApplyPagination(IQueryable<T> query)
	{
		return query
			.Skip(PageNumber - 1)
			.Take(PageSize);
	}
}
