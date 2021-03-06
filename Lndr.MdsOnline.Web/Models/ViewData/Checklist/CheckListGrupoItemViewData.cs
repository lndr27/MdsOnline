﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lndr.MdsOnline.Web.Models.ViewData.CheckList
{
    public class CheckListGrupoItemViewData
    {
        public CheckListGrupoItemViewData()
        {
            this.Itens = new List<CheckListItemViewData>();
        }

        public string CheckListGrupoItemID { get; set; }

        [Required]
        public string Nome { get; set; }

        public string Descricao { get; set; }

        [Required]
        public List<CheckListItemViewData> Itens { get; set; }
    }
}