using API.Common.Paging;

namespace API.Dto.Auth
{
    public class UserRequestDto : PagingRequetBase
    {
        //Sorting

        public string SortBy { get; set; }

        //Filter
        public string KeySearch { get; set; }
    }
}
