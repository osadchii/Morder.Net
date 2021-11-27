using System.Runtime.Serialization;
using MediatR;

namespace Infrastructure.MediatR.Companies.Commands;

[DataContract]
public class UpdateCompanyInformation : IRequest
{
    [DataMember] public string Name { get; }

    [DataMember] public string Shop { get; }

    [DataMember] public string Url { get; }

    public UpdateCompanyInformation(string name, string shop, string url)
    {
        Name = name;
        Shop = shop;
        Url = url;
    }
}