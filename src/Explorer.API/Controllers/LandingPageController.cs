using Explorer.API.Dtos.Tours;
using Explorer.Blog.API.Dtos;
using Explorer.Blog.API.Public;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Public;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers
{
    [Route("api/langing-page")]
    public class LandingPageController : BaseApiController
    {
        private readonly IBlogPostService _blogPostService;
        private readonly IApplicationGradeService _applicationGradeService;


        public LandingPageController(IBlogPostService blogPostService, IApplicationGradeService applicationGradeService)
        {
            _blogPostService = blogPostService;
            _applicationGradeService = applicationGradeService;
        }

        [HttpGet("top-rated-blogs/{count}")]
        public ActionResult<PagedResult<BlogPostDto>> GetTopRatedBlogPosts(int count)
        {
            var result = _blogPostService.GetTopRatedBlogPosts(count);
            return CreateResponse(result);
        }

        [HttpGet("app-rating-exists/{id}")]
        public bool Exists(int id)
        {
            return _applicationGradeService.Exists(id);
        }
    }
}
