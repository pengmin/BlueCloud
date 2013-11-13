using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ufida.T.BAP.Base;
using Ufida.T.BAP.Web.Voucher.VoucherEdit;
using Ufida.T.BAP.Web.Voucher.VoucherEdit.Action;
using Ufida.T.EAP.Domains;
using Ufida.T.SA.DTO;
using Ufida.T.SA.Interface;

namespace chanjet.SAAppTest5.Web
{
	public class TestActionCreator : Ufida.T.EAP.AppBase.IAppHandler
	{
		public void AppEventHandler(object sender, Ufida.T.EAP.AppBase.AppEventArgs e)
		{
			e.Data = new TestAction<ISaleOrderService, SaleOrderDTO>();
		}
	}

	public class TestAction<I, M> : VoucherEditActionBase<I, M>
		where M : DTO, new()
		where I : class, IVoucherService<M>
	{
		protected override void ExecuteVoucherEditAction(VoucherController<I, M> ctler)
		{
			M dto = ctler.GetInstance(ctler.InitArgument);
			ctler.SetDTO(dto);
			this.AjaxContext.ResponseData.Script.Add("alert('TestAction');");
		}
	}
}
