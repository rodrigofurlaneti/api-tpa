import { Component, ViewChild } from '@angular/core';
import { IonicPage, NavController, NavParams } from 'ionic-angular';
import { HttpProvider } from '../../../providers/http/http';
import { PeopleProvider } from '../../../providers/people/people';
import { UtilsProvider } from '../../../providers/utils/utils';
import { PaymentProvider } from '../../../providers/payment/payment';
import { Slides } from 'ionic-angular';
import { ProductPlanProvider } from '../../../providers/product-plan/product-plan';
import { OrderProvider } from '../../../providers/order/order';
import { ShoppingProvider } from '../../../providers/shopping/shopping';
import { FormGroup, FormControl, Validators } from '@angular/forms';

/**
 * Generated class for the ProductPlanOrderPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */

@IonicPage()
@Component({
  selector: 'page-product-plan-order',
  templateUrl: 'product-plan-order.html',
})
export class ProductPlanOrderPage {

  @ViewChild(Slides) slides: Slides;

  planos: any;
  cartoes: any;
  agendamentos: any[] = [];
  pedido: any = {
    ListaCompra: null,
    Cartao: { Id: 0, Senha: '' },
    Endereco: {},
    Agendamento: {},
    Usuario: { Id: HttpProvider.userAuth.UsuarioId },
    Valor: '',
    Loja: { Id: 0 }
  };
  listaCompra: any;
  valorTotal: number = 0;
  cartao: any;
  sendedDependente: boolean = false;
  exibirDependente: boolean = false;
  exibirParametros: boolean = false;
  planoSelecionado: any;
  planoSelecionardoTitular: any;
  percentual: number;

  clientForm: FormGroup = new FormGroup(
    {
      id: new FormControl(),
      cpf: new FormControl('', [Validators.required]),
      nome: new FormControl('', [Validators.required]),
      sexo: new FormControl(''),
      dataNascimento: new FormControl(''),
      celular: new FormControl(''),
      email: new FormControl('', [Validators.required, Validators.email])
    }
  );

  constructor(public navCtrl: NavController,
    private productPlanProvider: ProductPlanProvider,
    private paymentProvider: PaymentProvider,
    private utils: UtilsProvider,
    private orderProvider: OrderProvider,
    private peopleProvider: PeopleProvider,
    private shoppingProvider: ShoppingProvider,
    public navParams: NavParams) {
    this.pedido.Loja = this.navParams.get("loja");
  }

  ionViewDidLoad() {
    if (!HttpProvider.userAuth) {
      this.utils.showAlert('Atenção', 'Cadastre-se ou logue-se para acidicionar o produto ao carrinho.');
      this.navCtrl.setRoot("LoginPage", { "redirect": "ProductPlanOrderPage" });
      return;
    }

    this.productPlanProvider.getAll()
      .subscribe(
        data => {
          if (data)
            this.planos = data;
        }, error => {
          this.utils.showToast(error.error);
        }
      );

    this.loadCartoes();
  }

  ionViewWillEnter() {
    this.loadLista();
  }

  loadCartoes(): any {
    this.paymentProvider.loadCartoes()
      .subscribe(
        data => {
          this.cartoes = data;
          if (data.find(x => x.TipoCartao.Id == 4))
            this.pedido.Cartao = data.find(x => x.TipoCartao.Id == 4);
          else
            this.utils.showError("Erro ao carregar cartão de adiantamento, contate o suporte!");
        }
      )
  }

  loadLista(): any {
    this.utils.showLoader('Carregando...');

    this.shoppingProvider.getListaAtivaLoja({ Id: 13372 })
      .subscribe(
        data => {
          this.pedido.ListaCompra = ShoppingProvider.getCurrentLista();
          this.pedido.Usuario = { Id: HttpProvider.userAuth.UsuarioId }
          this.valorTotal = this.pedido.ListaCompra ? this.pedido.ListaCompra.Total : 0;
          this.pedido.Valor = this.valorTotal;
          this.pedido.Loja = { Id: 13372 }
          this.utils.hideLoader();
        },
        error => {
          this.utils.hideLoader();
          this.utils.showError(error.message)
        }
      )

    this.utils.hideLoader();
  }

  confirmarCompra() {
    this.utils.showLoader("finalizando compra");
    this.orderProvider.finalizePlanoVenda(this.pedido)
      .subscribe(
        data => {
          this.utils.hideLoader();
          this.utils.showAlert('Sucesso', data.Mensagem);
          this.orderProvider.get(data.ObjetoRetorno.Id).subscribe(data => {
            this.cleanPedido();
            ShoppingProvider.setCurrentLista(null);
            this.loadLista();
          });

        }, error => {
          this.utils.showError("Erro ao processar o pagamento, contate o suporte! ");
        }
      )
  }

  private cleanPedido() {
    this.pedido = {
      ListaCompra: null,
      Cartao: {},
      Endereco: {},
      Agendamento: {},
      Usuario: { Id: HttpProvider.userAuth.UsuarioId }
    };
  }

  exibirFormDependente(plano) {
    this.exibirDependente = true;
    this.planoSelecionado = plano;
  }

  cancelaDependente() {
    this.exibirDependente = false;
    this.clientForm.reset();
    this.planoSelecionado = null;
  }

  adicionarDependete() {
    this.sendedDependente = true;
    if (this.clientForm.valid) {
      this.utils.showLoader("Registrando dados!");
      const _data = this.formToUserData();

      this.peopleProvider.save(_data)
        .subscribe(data => {
          this.utils.hideLoader();
          this.assinar(this.planoSelecionado, data)
          this.cancelaDependente();
        }, error => {
          this.utils.hideLoader();
          this.utils.showToast(error.error);
        });
    } else {
      this.utils.showToast("Preencha os campos corretamente!");
    }
  }

  prepareForm(data: any): void {
    this.utils.hideLoader();
    this.clientForm.setValue(
      {
        id: data.Id,
        cpf: data.Cpf,
        nome: data.Nome,
        sexo: data.Sexo,
        dataNascimento: this.utils.formatData(data.DataNascimento),
        celular: data.Contatos && data.Contatos.length && data.Contatos.length > 1 && data.Contatos[1].Contato && data.Contatos[1].Contato.Numero ? data.Contatos[1].Contato.Numero : '',
        email: data.Email
      }
    )
  }

  private formToUserData(): any {

    const _formData = this.clientForm.value;

    const _pessoa: any = {
      Nome: _formData.nome,
      DataNascimento: this.utils.dataToFromBody(_formData.dataNascimento),
      Sexo: _formData.sexo,
    }

    let data: any = {
      Pessoa: {
        Nome: _formData.nome,
        DataNascimento: this.utils.dataToFromBody(_formData.dataNascimento),
        Sexo: _formData.sexo,
        Documentos: [
          {
            Numero: _formData.cpf,
            Tipo: 2,
            Id: 0,
            Pessoa: _pessoa
          }
        ],
        Contatos: [
          {
            Contato: {
              Email: _formData.email,
              Tipo: 1,
              Id: 0,
              Pessoa: _pessoa

            }
          },
          {
            Contato: {
              Numero: _formData.celular,
              Tipo: 3,
              Id: 0,
              Pessoa: _pessoa
            }
          }
        ]
      }
    }
    data.Pessoa.Id = _formData.id

    return data;
  }

  searchFunc(event: any) {
    this.utils.showLoader("Verificando CPF na base...")
    var cpf = event.target.value;

    this.peopleProvider.getPessoaByCPF(cpf)
      .subscribe(
        data => {
          if (data) {
            if (event.target.parentElement.id == "cpfProprietario") {
              this.prepareForm(data);
            }

            this.utils.showToast("Cadastro encontrado!");
            this.utils.hideLoader();
          }
          else {
            this.utils.showToast("CPF não cadastrado, favor continuar o cadastro...")
            this.utils.hideLoader();
          }
        },
        error => {
          this.utils.showToast("CPF não cadastrado, favor continuar o cadastro...")
          this.utils.hideLoader();
        }
      )
  }

  assinar(plano, pessoaId = null) {
    if (plano) {
      this.adicionarCarrinho(plano, { Id: pessoaId ? pessoaId : HttpProvider.userAuth.PessoaId });
    }
  }

  adicionarCarrinho(produtoMv, pessoa) {
    let product = produtoMv;
    try {
      product = this.produtoToServerData(produtoMv, pessoa);
    } catch (error) { }
    this.utils.showLoader("inserido combo...");
    this.shoppingProvider.adicionarItem(product)
      .subscribe(
        data => {
          this.utils.hideLoader();
          this.valorTotal += produtoMv.ValorDesconto != 0 ? produtoMv.ValorDesconto : produtoMv.Valor;
          this.utils.showToast("Item adicionado ao carrinho!");
          this.loadLista();
        },
        error => { this.utils.hideLoader(); this.utils.showToast(error.error.Message) }
      )
  }

  removerDoCarrinho(produtoMv){
    this.shoppingProvider.removerItem(produtoMv).subscribe(
      data => {
        this.utils.hideLoader();
        this.valorTotal -= produtoMv.ValorDesconto != 0 ? produtoMv.ValorDesconto : produtoMv.Valor;
        this.utils.showToast("Item removido do carrinho!");
        this.loadLista();
      },
      error => { this.utils.hideLoader(); this.utils.showToast(error.error.Message) }
    )
  }

  produtoToServerData(plano, pessoa): any {
    return {
      PlanoVenda: plano,
      Beneficiario: pessoa,
      Quantidade: 1
    };
  }
}
