using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Domain.Common
{
    public enum GenreType
    {
        ScienceFiction = 1,
        Technical = 2,
        ChildrensBook = 3,
        Drama = 4,
        Thriller = 5,
        Novel = 6,
        History = 7,
        Art = 8,
        Gardening = 9
    }

    public enum OrderStatus
    {
        NotFinished = 1,
        Failed = 2,
        Received = 3,
        InDelivery = 3,
        FailedDelivery = 4,
        SuccessfulDelivery = 5
    }
}
