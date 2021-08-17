using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace API.Helpers
{
    public class PagedList<T> : List<T>
    {
        public PagedList(IEnumerable<T> items,int count,int pageNumber,int pageSize)
        {
            CurrentPage = pageNumber;
            TotalPages = (int) Math.Ceiling(count/(double)pageSize); //if have totalcount of 10, page size is 5, then 2  toatal pages, if have total count of 9, then page size size is 2
            PageSize = pageSize;
            TotalCount = count;
            AddRange(items);
        }     
      
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source,int pageNumber, int pageSize)
        {
            var count=await source.CountAsync();
            var items=await source.Skip((pageNumber-1)*pageSize).Take(pageSize).ToListAsync();// if your currentpagenumber is 2 and pagesize is 5. hence skip((2-1)*5).take(5). which means skip the first 5 records and take the next 5 records for page number 2. 
            
            return new PagedList<T>(items,count,pageNumber,pageSize);
        }
    }
}