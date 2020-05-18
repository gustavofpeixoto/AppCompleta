﻿using DevIO.App.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DevIO.App.ViewModels
{
    public class ProductViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("Fornecedor")]
        public Guid ProviderId { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "O campo {0} precisa ter entre {1} e {2} caracteres")]
        [DisplayName("Nome")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(1000, MinimumLength = 2, ErrorMessage = "O campo {0} precisa ter entre {1} e {2} caracteres")]
        [DisplayName("Descrição")]
        public string Description { get; set; }

        public string Picture { get; set; }

        [DisplayName("Nome do Produto")]
        public IFormFile PictureUpload { get; set; }

        [Currency]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("Preço")]
        public decimal Price { get; set; }

        [ScaffoldColumn(false)]
        public DateTime RegistrationDate { get; set; }

        [DisplayName("Ativo?")]
        public bool Active { get; set; }

        public ProviderViewModel Provider { get; set; }

        public IEnumerable<ProviderViewModel> Providers { get; set; }
    }
}
