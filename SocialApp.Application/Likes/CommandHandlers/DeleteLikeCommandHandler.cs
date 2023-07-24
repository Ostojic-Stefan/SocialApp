using AutoMapper;
using EfCoreHelpers;
using SocialApp.Application.Likes.Commands;
using SocialApp.Application.Likes.Responses;
using SocialApp.Application.Models;
using SocialApp.Domain;

namespace SocialApp.Application.Likes.CommandHandlers;

internal class DeleteLikeCommandHandler
    : DataContextRequestHandler<DeleteLikeCommand, Result<PostLikeResponse>>
{
    private readonly IMapper _mapper;

    public DeleteLikeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) 
        : base(unitOfWork)
    {
        _mapper = mapper;
    }

    public override async Task<Result<PostLikeResponse>> Handle(DeleteLikeCommand request,
        CancellationToken cancellationToken)
    {
        var result = new Result<PostLikeResponse>();
        try
        {
            var postLikeRepo = _unitOfWork.CreateReadWriteRepository<PostLike>();
            var deleted = await postLikeRepo.RemoveAsync(request.LikeId,
                cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);
            result.Data = _mapper.Map<PostLikeResponse>(deleted);
        }
        catch (Exception ex)
        {
            result.AddError(AppErrorCode.ServerError, ex.Message);
        }
        return result;
    }
}
