using iTextSharp.text;
using iTextSharp.text.pdf;
using JobRegistrationSubmisson.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebClient.PDF
{
    public class profilePDF
    {
        #region Declaration
        int _totalColumn = 2; //check
        Document _document;
        Font _fontSylye;
        PdfPTable _pdfTable = new PdfPTable(2); //check
        PdfPCell _pdfPCell;
        MemoryStream _memoryStream = new MemoryStream();
        List<JobSeekerVM> _forms = new List<JobSeekerVM>();

        #endregion

        public byte[] Prepare(List<JobSeekerVM> JobSeekers)
        {
            _forms = JobSeekers;

            //foreach (JobSeekerVM form in _forms)
            //{
            //    var name = form.Name;
            //    var gender = form.Gender;
            //    var birt_date = form.Birth_Date;
            //    var address = form.Address;
            //    var religion = form.Religion;
            //    var status = form.Marital_Status;
            //    var nationality = form.Nationality;
            //    var edu = form.Last_Education;
            //    var gpa = form.GPA;
            //    var tech = form.Technical_Skill;
            //    var exp = form.Experience;
            //    var ach = form.Achievement;
            //}

            #region
            _document = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _document.SetPageSize(PageSize.A4);
            _document.SetMargins(20f, 20f, 20f, 20f);
            _pdfTable.WidthPercentage = 100;
            _pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            _fontSylye = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_document, _memoryStream);
            _document.Open();
            _pdfTable.SetWidths(new float[] { 20f, 20f, 50f }); //check
            #endregion

            this.ReportHeader();
            this.ReportBody();
            _pdfTable.HeaderRows = 2;
            _document.Add(_pdfTable);
            _document.Close();
            return _memoryStream.ToArray();
        }

        private void ReportHeader()
        {

            _fontSylye = FontFactory.GetFont("Tahoma", 11f, 1);
            _pdfPCell = new PdfPCell(new Phrase("Forms Data", _fontSylye));
            _pdfPCell.Colspan = _totalColumn;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(_pdfPCell);
            _pdfTable.CompleteRow();

        }

        private void ReportBody()
        {

            //_fontSylye = FontFactory.GetFont("Tahoma", 11f, 1);
            //_pdfPCell = new PdfPCell((JobSInfo()));
            //_pdfPCell.Colspan = 1;
            //_pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //_pdfPCell.Border = 0;
            //_pdfPCell.BackgroundColor = BaseColor.WHITE;
            //_pdfTable.AddCell(_pdfPCell);
            //_pdfTable.CompleteRow();

            var number = 1;
            #region Table header1
            _fontSylye = FontFactory.GetFont("Tahoma", 8f, 1);

            //_pdfPCell = new PdfPCell(new Phrase("No", _fontSylye));
            //_pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //_pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //_pdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            ////_pdfPCell.Border = 0;

            ////_pdfPCell.Colspan = 1;

            ////_pdfPCell.Border = 0;

            //_pdfTable.AddCell(_pdfPCell);

            //_pdfPCell = new PdfPCell(new Phrase("ID", _fontSylye));
            //_pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //_pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //_pdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            //_pdfTable.AddCell(_pdfPCell);

            _pdfPCell = new PdfPCell(new Phrase("Jobseeker Name", _fontSylye));
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _pdfPCell.Border = 0;

            _pdfTable.AddCell(_pdfPCell);

            _pdfPCell = new PdfPCell(new Phrase("Joblist Name", _fontSylye));
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            //_pdfPCell.Border = 0;

            _pdfTable.AddCell(_pdfPCell);

            //_pdfPCell = new PdfPCell(new Phrase("Updated Date", _fontSylye));
            //_pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //_pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //_pdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            //_pdfTable.AddCell(_pdfPCell);

            _pdfTable.CompleteRow();
            #endregion

            #region Table Body
            _fontSylye = FontFactory.GetFont("Tahoma", 8f, 0);
            foreach (JobSeekerVM form in _forms)
            {
                _pdfPCell = new PdfPCell(new Phrase(number++.ToString(), _fontSylye));
                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                //_pdfPCell.Border = 0;

                _pdfTable.AddCell(_pdfPCell);

                _pdfPCell = new PdfPCell(new Phrase(form.Name, _fontSylye));
                // _pdfPCell.Border = 0;
                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                //_pdfPCell.Border = 0;

                _pdfTable.AddCell(_pdfPCell);

                _pdfPCell = new PdfPCell(new Phrase(form.JoblistName, _fontSylye));
                // _pdfPCell.Border = 0;
                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                //_pdfPCell.Border = 0;

                _pdfTable.AddCell(_pdfPCell);
                //_pdfPCell = new PdfPCell(new Phrase(form.Id.ToString(), _fontSylye));
                //_pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                //_pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                //_pdfPCell.BackgroundColor = BaseColor.WHITE;
                //_pdfTable.AddCell(_pdfPCell);

                //_pdfPCell = new PdfPCell(new Phrase(form.department.Name, _fontSylye));
                //_pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                //_pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                //_pdfPCell.BackgroundColor = BaseColor.WHITE;
                //_pdfTable.AddCell(_pdfPCell);

                //_pdfPCell = new PdfPCell(new Phrase(form.CreateData.ToString(), _fontSylye));
                //_pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                //_pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                //_pdfPCell.BackgroundColor = BaseColor.WHITE;
                //_pdfTable.AddCell(_pdfPCell);

                //_pdfPCell = new PdfPCell(new Phrase(form.UpdateDate.ToString(), _fontSylye));
                //_pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                //_pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                //_pdfPCell.BackgroundColor = BaseColor.WHITE;
                //_pdfTable.AddCell(_pdfPCell);

            }
            _pdfTable.CompleteRow();
            #endregion
        }
    }
}
