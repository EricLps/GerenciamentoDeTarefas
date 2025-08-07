using System;
using System.ComponentModel.DataAnnotations; // Necessário para a anotação [Key]

namespace GerenciadorDeTarefas.Models
{
    public class Tarefa
    {
        [Key]
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public DateTime Data { get; set; }
        public EnumStatusTarefa Status { get; set; }
    }
}