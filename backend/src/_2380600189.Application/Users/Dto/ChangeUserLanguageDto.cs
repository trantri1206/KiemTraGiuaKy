using System.ComponentModel.DataAnnotations;

namespace _2380600189.Users.Dto;

public class ChangeUserLanguageDto
{
    [Required]
    public string LanguageName { get; set; }
}