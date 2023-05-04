using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrpcHelper.AuthService;
using Models.Models;

namespace Models.Mapping
{
    public static class FilterMapping
    {
        public static Filter ToModel(this GrpcHelper.DbService.Filter grpcFilter)
        {
            return new Filter
            {
                IgnoreVideo = grpcFilter.IgnoreVideo,
                IgnoreRepost = grpcFilter.IgnoreRepost,
                IgnoreDescriptions = grpcFilter.IgnoreDescriptions.ToList(),
                IgnoreAuthors = grpcFilter.IgnoreAuthors.ToList(),
                IgnoreTitles = grpcFilter.IgnoreTitles.ToList(),
                IgnoreWords = grpcFilter.IgnoreWords.ToList(),
            };
        }

        public static GrpcHelper.DbService.Filter ToGrpcData(this Filter filter)
        {
            return new GrpcHelper.DbService.Filter
            {
                IgnoreVideo = filter.IgnoreVideo,
                IgnoreRepost = filter.IgnoreRepost,
                IgnoreDescriptions = { filter.IgnoreDescriptions },
                IgnoreAuthors = { filter.IgnoreAuthors },
                IgnoreTitles = { filter.IgnoreTitles },
                IgnoreWords = { filter.IgnoreWords },
            };
        }
    }
}
