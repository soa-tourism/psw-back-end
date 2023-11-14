﻿using Explorer.Blog.API.Public;
using Explorer.Blog.API.Dtos;
using Explorer.BuildingBlocks.Core.UseCases;
using AutoMapper;
using FluentResults;
using Explorer.Blog.Core.Domain.RepositoryInterfaces;
using Explorer.Blog.Core.Domain.BlogPosts;

namespace Explorer.Blog.Core.UseCases;

public class BlogCommentService : BaseService<BlogPostDto, BlogPost>, IBlogCommentService
{
    private readonly IMapper _mapper;
    private readonly IBlogPostRepository _blogPostsRepository;

    public BlogCommentService(IBlogPostRepository repository, IMapper mapper) : base(mapper)
    {
        _mapper = mapper;
        _blogPostsRepository = repository;
    }

    public Result<BlogPostDto> Add(int blogPostId, BlogCommentDto blogCommentDto)
    {
        try
        {
            var blogPost = _blogPostsRepository.Get(blogPostId);
            var blogComment = _mapper.Map<BlogCommentDto, BlogComment>(blogCommentDto);

            blogPost.AddComment(blogComment);
            var result = _blogPostsRepository.Update(blogPost);
            return MapToDto(result);
        }
        catch (KeyNotFoundException e)
        {
            return Result.Fail(FailureCode.NotFound).WithError(e.Message);
        }
        catch (ArgumentException e)
        {
            return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
        }
    }

    public Result Remove(int blogPostId, BlogCommentDto blogCommentDto)
    {
        try
        {
            var blogPost = _blogPostsRepository.Get(blogPostId);
            var blogComment = _mapper.Map<BlogCommentDto, BlogComment>(blogCommentDto);

            blogPost.DeleteComment(blogComment);
            _blogPostsRepository.Update(blogPost);
            return Result.Ok();
        }
        catch (KeyNotFoundException e)
        {
            return Result.Fail(FailureCode.NotFound).WithError(e.Message);
        }
        catch (ArgumentException e)
        {
            return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
        }
    }
}