using System.ComponentModel.DataAnnotations.Schema;

namespace FakeApi.Entities;

public class Album : EntityBase
{
    public string Title { get; set; }
    public int UserId { get; set; }
}