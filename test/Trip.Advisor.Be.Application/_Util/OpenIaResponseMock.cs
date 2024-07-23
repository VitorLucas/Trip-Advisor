using Trip.Advisor.Be.Core.Helpers;

namespace  Trip.Advisor.Be.Application._Util;

public static class OpenIaResponseMock
{
    public static Result CreateOpenIaValidResult()
    {
        return new Result(CreateOpenIaValidResponse(), System.Net.HttpStatusCode.OK);
    }

    public static Result CreateOpenIaInvalidResult()
    {
        return new Result(CreateOpenIaInvalidResponse(), System.Net.HttpStatusCode.OK);
    }

    public static Result CreateOpenIaNullContentResult()
    {
        return new Result(null, System.Net.HttpStatusCode.NotFound);
    }

    public static Result CreateOpenIaEmptyContentResult()
    {
        return new Result(string.Empty, System.Net.HttpStatusCode.NotFound);
    }

    public static string CreateOpenIaValidResponse()
    {
        return "{\n    \"destiny\": \"Portugal\",\n    \"period\": \"5 days\",\n    \"amount\": {\n        \"accommodation\": 100,\n        \"restaurants\": 100\n    },\n    \"days\": [\n        {\n            \"day\": 1,\n            \"activities\": [\n                {\n                    \"name\": \"Lisbon\",\n                    \"detail\": \"Explore the historic neighborhoods of Alfama and Bairro Alto, visit Belem Tower and Jeronimos Monastery\"\n                }\n            ]\n        },\n        {\n            \"day\": 2,\n            \"activities\": [\n                {\n                    \"name\": \"Sintra\",\n                    \"detail\": \"Visit the colorful Pena Palace, explore the Moorish Castle and stroll through the charming town center\"\n                }\n            ]\n        },\n        {\n            \"day\": 3,\n            \"activities\": [\n                {\n                    \"name\": \"Porto\",\n                    \"detail\": \"Discover the picturesque Ribeira district, visit Livraria Lello bookshop and enjoy a Port wine tasting\"\n                }\n            ]\n        },\n        {\n            \"day\": 4,\n            \"activities\": [\n                {\n                    \"name\": \"Douro Valley\",\n                    \"detail\": \"Take a scenic boat cruise on the Douro River, visit vineyards and enjoy wine tasting\"\n                }\n            ]\n        },\n        {\n            \"day\": 5,\n            \"activities\": [\n                {\n                    \"name\": \"Algarve\",\n                    \"detail\": \"Relax on the beautiful beaches, explore the caves and cliffs of Lagos, and enjoy fresh seafood by the coast\"\n                }\n            ]\n        }\n    ]\n}";
    }

    public static string CreateOpenIaInvalidResponse()
    {
        return ": \"5 days\",\n    \"amount\": {\n        \"accommodation\": 100,\n        \"restaurants\": 100\n    },\n    \"days\": [\n        {\n            \"day\": 1,\n            \"activities\": [\n                {\n                    \"name\": \"Lisbon\",\n                    \"detail\": \"Explore the historic neighborhoods of Alfama and Bairro Alto, visit Belem Tower and Jeronimos Monastery\"\n                }\n            ]\n        },\n        {\n            \"day\": 2,\n            \"activities\": [\n                {\n                    \"name\": \"Sintra\",\n                    \"detail\": \"Visit the colorful Pena Palace, explore the Moorish Castle and stroll through the charming town center\"\n                }\n            ]\n        },\n        {\n            \"day\": 3,\n            \"activities\": [\n                {\n                    \"name\": \"Porto\",\n                    \"detail\": \"Discover the picturesque Ribeira district, visit Livraria Lello bookshop and enjoy a Port wine tasting\"\n                }\n            ]\n        },\n        {\n            \"day\": 4,\n            \"activities\": [\n                {\n                    \"name\": \"Douro Valley\",\n                    \"detail\": \"Take a scenic boat cruise on the Douro River, visit vineyards and enjoy wine tasting\"\n                }\n            ]\n        },\n        {\n            \"day\": 5,\n            \"activities\": [\n                {\n                    \"name\": \"Algarve\",\n                    \"detail\": \"Relax on the beautiful beaches, explore the caves and cliffs of Lagos, and enjoy fresh seafood by the coast\"\n                }\n            ]\n        }\n    ]\n}";
    }
}
