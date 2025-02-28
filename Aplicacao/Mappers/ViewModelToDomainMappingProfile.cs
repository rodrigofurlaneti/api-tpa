﻿using Aplicacao.ViewModels;
using AutoMapper;
using Entidade;

namespace Aplicacao.Mappers
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        // Não realizar este override na versão 4.x e superiores
        public override string ProfileName => "ViewModelToDomainMappings";

        protected void Configure()
        {
            CreateMap<PessoaViewModel, Pessoa>().MaxDepth(1);
            CreateMap<EnderecoViewModel, Endereco>().MaxDepth(1);
            CreateMap<DocumentoViewModel, Documento>().MaxDepth(1);
            CreateMap<ContatoViewModel, Contato>().MaxDepth(1);
            CreateMap<UsuarioViewModel, Usuario>().MaxDepth(1);

            CreateMap<PerfilViewModel, Perfil>().ForMember(x => x.Usuarios, opt => opt.MapFrom(o => o.Usuarios)).MaxDepth(1);
            CreateMap<PermissaoViewModel, Permissao>().ForMember(x => x.Perfis, opt => opt.MapFrom(o => o.Perfis)).MaxDepth(1);
            CreateMap<PerfilMenuViewModel, PerfilMenu>().MaxDepth(1);

            CreateMap<MenuViewModel, Menu>().ForMember(x => x.MenuPai, opt => opt.MapFrom(o => o.MenuPai)).MaxDepth(1);

            CreateMap<CidadeViewModel, Cidade>();
            CreateMap<EstadoViewModel, Estado>();
            CreateMap<PaisViewModel, Pais>();
        }
    }
}
