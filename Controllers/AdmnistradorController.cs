using RoleTopMvc.Enums;
using RoleTopMvc.Repositories;
using RoleTopMvc.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace RoleTopMvc.Controllers {
    public class AdministradorController : AbstractController {
        AgendamentoRepository agendamentoRepository = new AgendamentoRepository ();
        public IActionResult Dashboard () {

            var ninguemLogado = string.IsNullOrEmpty(ObterUsuarioTipoSession());

            if (!ninguemLogado && 
            (uint) TiposUsuario.ADMINISTRADOR == uint.Parse(ObterUsuarioTipoSession())) {

                var pedidos = agendamentoRepository.ObterAgendamentos ();
                DashboardViewModel dashboardViewModel = new DashboardViewModel ();

                foreach (var pedido in pedidos) {
                    switch (pedido.Status) {
                        case (uint) StatusPedido.APROVADO:
                            dashboardViewModel.PedidosAprovados++;
                            break;
                        case (uint) StatusPedido.REPROVADO:
                            dashboardViewModel.PedidosReprovados++;
                            break;
                        default:
                            dashboardViewModel.PedidosPendentes++;
                            dashboardViewModel.Pedidos.Add (pedido);
                            break;
                    }
                }
                dashboardViewModel.NomeView = "Dashboard";
                dashboardViewModel.UsuarioEmail = ObterUsuarioSession ();

                return View (dashboardViewModel);
            } 
            else 
            {
                return View ("Erro", new RespostaViewModel(){
                    NomeView = "Dashboard",
                    Mensagem = "Você não tem permissão para acessar o Dashboard"
                });

            }
        }
    }
}