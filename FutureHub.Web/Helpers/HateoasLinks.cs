using FutureHub.Web.Models.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace FutureHub.Web.Helpers;

public static class HateoasLinks
{
    public static PagedResult<T> CreatePagedResult<T>(
        IEnumerable<T> data,
        int page,
        int pageSize,
        int totalCount,
        HttpRequest request,
        string routeName)
    {
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        var hasPrevious = page > 1;
        var hasNext = page < totalPages;

        var result = new PagedResult<T>
        {
            Data = data,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages,
            HasPrevious = hasPrevious,
            HasNext = hasNext,
            Links = new List<Link>()
        };

        // Self link
        result.Links.Add(new Link
        {
            Href = CreatePageUri(request, page, pageSize),
            Rel = "self",
            Method = "GET"
        });

        // First link
        result.Links.Add(new Link
        {
            Href = CreatePageUri(request, 1, pageSize),
            Rel = "first",
            Method = "GET"
        });

        // Previous link
        if (hasPrevious)
        {
            result.Links.Add(new Link
            {
                Href = CreatePageUri(request, page - 1, pageSize),
                Rel = "previous",
                Method = "GET"
            });
        }

        // Next link
        if (hasNext)
        {
            result.Links.Add(new Link
            {
                Href = CreatePageUri(request, page + 1, pageSize),
                Rel = "next",
                Method = "GET"
            });
        }

        // Last link
        result.Links.Add(new Link
        {
            Href = CreatePageUri(request, totalPages, pageSize),
            Rel = "last",
            Method = "GET"
        });

        return result;
    }

    private static string CreatePageUri(HttpRequest request, int page, int pageSize)
    {
        var baseUri = $"{request.Scheme}://{request.Host}{request.Path}";
        var queryParams = new List<string>();

        // Preservar query parameters existentes (exceto page e pageSize)
        foreach (var key in request.Query.Keys)
        {
            if (key.ToLower() != "page" && key.ToLower() != "pagesize")
            {
                queryParams.Add($"{key}={request.Query[key]}");
            }
        }

        // Adicionar novos page e pageSize
        queryParams.Add($"page={page}");
        queryParams.Add($"pageSize={pageSize}");

        return $"{baseUri}?{string.Join("&", queryParams)}";
    }

    public static List<Link> CreateResourceLinks(HttpRequest request, string resourceId, string controllerName)
    {
        var baseUri = $"{request.Scheme}://{request.Host}/api/{request.RouteValues["version"]}/{controllerName}";

        return new List<Link>
        {
            new Link
            {
                Href = $"{baseUri}/{resourceId}",
                Rel = "self",
                Method = "GET"
            },
            new Link
            {
                Href = $"{baseUri}/{resourceId}",
                Rel = "update",
                Method = "PUT"
            },
            new Link
            {
                Href = $"{baseUri}/{resourceId}",
                Rel = "delete",
                Method = "DELETE"
            }
        };
    }
}
