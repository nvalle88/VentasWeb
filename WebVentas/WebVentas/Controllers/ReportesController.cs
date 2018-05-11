using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using WebVentas.Utils;

namespace WebVentas.Controllers
{
    public class ReportesController : Controller
    {
        // GET: Reportes
        public ActionResult ReporteClientes()
        {
            ReportViewer rptViewer = new ReportViewer();

            // ProcessingMode will be Either Remote or Local  
            rptViewer.ProcessingMode = ProcessingMode.Remote;
            rptViewer.SizeToReportContent = true;
            rptViewer.ZoomMode = ZoomMode.PageWidth;
            rptViewer.Width = Unit.Percentage(100);
            rptViewer.Height = Unit.Percentage(100);
            rptViewer.AsyncRendering = true;


            IReportServerCredentials irsc = new CustomReportCredentials(Constantes.UsuarioReport, Constantes.Contrasena);

            rptViewer.ServerReport.ReportServerCredentials = irsc;

            rptViewer.ServerReport.ReportServerUrl = new Uri(Constantes.ServerReportUrl);

            rptViewer.ServerReport.ReportPath = Constantes.ReporteClientesPath;

            ViewBag.ReportViewer = rptViewer;
            return View();
        }
        public ActionResult ReporteCompromisos()
        {
            ReportViewer rptViewer = new ReportViewer();

            // ProcessingMode will be Either Remote or Local  
            rptViewer.ProcessingMode = ProcessingMode.Remote;
            rptViewer.SizeToReportContent = true;
            rptViewer.ZoomMode = ZoomMode.PageWidth;
            rptViewer.Width = Unit.Percentage(100);
            rptViewer.Height = Unit.Percentage(100);
            rptViewer.AsyncRendering = true;


            IReportServerCredentials irsc = new CustomReportCredentials(Constantes.UsuarioReport, Constantes.Contrasena);

            rptViewer.ServerReport.ReportServerCredentials = irsc;

            rptViewer.ServerReport.ReportServerUrl = new Uri(Constantes.ServerReportUrl);

            rptViewer.ServerReport.ReportPath = Constantes.ReporteCompromisosPath;

            ViewBag.ReportViewer = rptViewer;
            return View();
        }
        public ActionResult ReporteVendedores()
        {
            ReportViewer rptViewer = new ReportViewer();

            // ProcessingMode will be Either Remote or Local  
            rptViewer.ProcessingMode = ProcessingMode.Remote;
            rptViewer.SizeToReportContent = true;
            rptViewer.ZoomMode = ZoomMode.PageWidth;
            rptViewer.Width = Unit.Percentage(100);
            rptViewer.Height = Unit.Percentage(100);
            rptViewer.AsyncRendering = true;


            IReportServerCredentials irsc = new CustomReportCredentials(Constantes.UsuarioReport, Constantes.Contrasena);

            rptViewer.ServerReport.ReportServerCredentials = irsc;

            rptViewer.ServerReport.ReportServerUrl = new Uri(Constantes.ServerReportUrl);

            rptViewer.ServerReport.ReportPath = Constantes.ReporteVendedoresPath;

            ViewBag.ReportViewer = rptViewer;
            return View();
        }
    }
}