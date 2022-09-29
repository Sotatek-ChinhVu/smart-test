using DevExpress.DataAccess.ObjectBinding;
using DevExpress.Models;
using DevExpress.Template;
using DevExpress.Xpo.Helpers;
using DevExpress.XtraPrinting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.MedicalExamination.GetHistory;

namespace DevExpress.Export
{
    public class Karte2Export
    {
        public void ExportToPdf()
        {
            var report = new Karte2Template();
            var dataSource = new ObjectDataSource();
            string name = "<div class=\"figure\">\r\n  <img\r\n    src=\"https://d27jswm5an3efw.cloudfront.net/app/uploads/2019/07/insert-image-html.jpg\"\r\n    alt=\"The head and torso of a dinosaur skeleton;\r\n            it has a large head with long sharp teeth\"\r\n    width=\"400\"\r\n    height=\"341\" />\r\n\r\n  <p>A T-Rex on display in the Manchester University Museum.</p>\r\n</div>\r\n<h1>This is heading 1</h1>\r\n<h2>This is heading 2</h2>\r\n<h3>This is heading 3</h3>\r\n<h4>This is heading 4</h4>\r\n<h5>This is heading 5</h5>\r\n<h6>This is heading 6</h6>\r\n<p><b>Tip:</b> Use h1 to h6 elements only for headings. Do not use them just to make text bold or big. Use other tags for that.</p>\r\n<h1 style=\"color:blue;text-align:center;\">This is a heading</h1>\r\n<p style=\"color:red;\">This is a paragraph.</p>\r\n<p style=\"color:green;\">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris ut nunc ullamcorper, tristique magna eu, bibendum orci. Integer sit amet sapien odio. Aliquam tincidunt nec augue quis volutpat. Praesent maximus gravida odio, ut sodales justo vulputate in. Integer imperdiet elementum mauris, ut luctus nibh laoreet in. Donec gravida tempor efficitur. Maecenas tempor ante ante, at posuere sapien finibus quis. Nam at nisl vitae massa tincidunt volutpat id a est. Integer lobortis in nisl ut varius. Morbi accumsan dui ante, ut volutpat felis aliquam ut. Aliquam rhoncus posuere fermentum. In a nunc sed neque hendrerit fringilla vel porta elit. Nulla congue sit amet nisi non ultrices. Cras pretium id ante vitae maximus. Quisque maximus ipsum condimentum eros accumsan dictum. Curabitur ac fermentum neque.\r\n\r\nPraesent elementum nunc ut felis porttitor, et iaculis augue posuere. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae; Curabitur sagittis arcu lorem, in semper sem egestas quis. Nam posuere turpis sit amet risus feugiat, quis posuere orci maximus. Ut tellus sem, maximus ut fermentum id, ultrices vel augue. Pellentesque sodales et enim vitae pulvinar. In et luctus justo. Etiam libero purus, elementum sit amet magna eu, eleifend pellentesque mauris. Pellentesque aliquet malesuada rhoncus. Pellentesque aliquet diam a velit porta, vitae pharetra felis commodo.\r\n\r\nProin ultricies felis ut nulla maximus, at tincidunt odio mattis. Ut eget magna enim. Aliquam porta eu risus vel volutpat. Cras accumsan facilisis leo et varius. Ut ac nibh cursus, consequat dolor eget, interdum diam. Donec egestas sapien eu mauris placerat, quis pellentesque ante interdum. Sed hendrerit eget turpis id gravida. Curabitur eu vestibulum mauris. Cras pharetra mattis sapien, ut gravida mauris. Etiam malesuada tellus augue, id venenatis arcu posuere at. Aliquam luctus auctor quam id pharetra. Nam rhoncus ligula vel nunc aliquet sodales. Nulla et nunc molestie, dapibus augue ac, aliquet orci. Nullam turpis nulla, fringilla quis ligula ac, maximus fringilla lacus.\r\n\r\nSed egestas fringilla dolor, non faucibus felis lobortis vel. Morbi rhoncus eros non orci consectetur hendrerit. Cras nisi eros, blandit bibendum aliquam eget, congue ut urna. In interdum, eros ut placerat ultricies, eros nulla egestas nunc, ac vulputate Sed egestas fringilla dolor, non faucibus felis lobortis vel. Morbi rhoncus eros non orci consectetur hendrerit. Cras nisi eros, blandit bibendum aliquam eget, congue ut urna. In interdum, eros ut placerat ultricies, eros nulla egestas nunc, ac vulputate lacus risus vitae arcu. Donec iaculis venenatis eros, non convallis leo. Proin viverra consectetur ullamcorper. Praesent fringilla auctor consectetur. Donec vel leo mauris. Etiam tincidunt, libero eu efficitur ultrices, augue nisl suscipit lectus, id condimentum sapien justo nec orci. Mauris ultricies, tortor et auctor vestibulum, turpis enim sodales eros, lacinia blandit diam urna at metus. Aliquam risus libero, euismod vel lacinia sodales, dignissim ut libero. Nam at ligula risus. Aliquam rhoncus ullamcorper vestibulum. Ut sit amet mi nec dui aliquam congue ac vitae sapien. Proin hendrerit consequat posuere.\r\nSed egestas fringilla dolor, non faucibus felis lobortis vel. Morbi rhoncus eros non orci consectetur hendrerit. Cras nisi eros, blandit bibendum aliquam eget, congue ut urna. In interdum, eros ut placerat ultricies, eros nulla egestas nunc, ac vulputate lacus risus vitae arcu. Donec iaculis venenatis eros, non convallis leo. Proin viverra consectetur ullamcorper. Praesent fringilla auctor consectetur. Donec vel leo mauris. Etiam tincidunt, libero eu efficitur ultrices, augue nisl suscipit lectus, id condimentum sapien justo nec orci. Mauris ultricies, tortor et auctor vestibulum, turpis enim sodales eros, lacinia blandit diam urna at metus. Aliquam risus libero, euismod vel lacinia sodales, dignissim ut libero. Nam at ligula risus. Aliquam rhoncus ullamcorper vestibulum. Ut sit amet mi nec dui aliquam congue ac vitae sapien. Proin hendrerit consequat posuere.\r\nSed egestas fringilla dolor, non faucibus felis lobortis vel. Morbi rhoncus eros non orci consectetur hendrerit. Cras nisi eros, blandit bibendum aliquam eget, congue ut urna. In interdum, eros ut placerat ultricies, eros nulla egestas nunc, ac vulputate lacus risus vitae arcu. Donec iaculis venenatis eros, non convallis leo. Proin viverra consectetur ullamcorper. Praesent fringilla auctor consectetur. Donec vel leo mauris. Etiam tincidunt, libero eu efficitur ultrices, augue nisl suscipit lectus, id condimentum sapien justo nec orci. Mauris ultricies, tortor et auctor vestibulum, turpis enim sodales eros, lacinia blandit diam urna at metus. Aliquam risus libero, euismod vel lacinia sodales, dignissim ut libero. Nam at ligula risus. Aliquam rhoncus ullamcorper vestibulum. Ut sit amet mi nec dui aliquam congue ac vitae sapien. Proin hendrerit consequat posuere.\r\nSed egestas fringilla dolor, non faucibus felis lobortis vel. Morbi rhoncus eros non orci consectetur hendrerit. Cras nisi eros, blandit bibendum aliquam eget, congue ut urna. In interdum, eros ut placerat ultricies, eros nulla egestas nunc, ac vulputate lacus risus vitae arcu. Donec iaculis venenatis eros, non convallis leo. Proin viverra consectetur ullamcorper. Praesent fringilla auctor consectetur. Donec vel leo mauris. Etiam tincidunt, libero eu efficitur ultrices, augue nisl suscipit lectus, id condimentum sapien justo nec orci. Mauris ultricies, tortor et auctor vestibulum, turpis enim sodales eros, lacinia blandit diam urna at metus. Aliquam risus libero, euismod vel lacinia sodales, dignissim ut libero. Nam at ligula risus. Aliquam rhoncus ullamcorper vestibulum. Ut sit amet mi nec dui aliquam congue ac vitae sapien. Proin hendrerit consequat posuere.\r\nSed egestas fringilla dolor, non faucibus felis lobortis vel. Morbi rhoncus eros non orci consectetur hendrerit. Cras nisi eros, blandit bibendum aliquam eget, congue ut urna. In interdum, eros ut placerat ultricies, eros nulla egestas nunc, ac vulputate lacus risus vitae arcu. Donec iaculis venenatis eros, non convallis leo. Proin viverra consectetur ullamcorper. Praesent fringilla auctor consectetur. Donec vel leo mauris. Etiam tincidunt, libero eu efficitur ultrices, augue nisl suscipit lectus, id condimentum sapien justo nec orci. Mauris ultricies, tortor et auctor vestibulum, turpis enim sodales eros, lacinia blandit diam urna at metus. Aliquam risus libero, euismod vel lacinia sodales, dignissim ut libero. Nam at ligula risus. Aliquam rhoncus ullamcorper vestibulum. Ut sit amet mi nec dui aliquam congue ac vitae sapien. Proin hendrerit consequat posuere.\r\nlacus risus vitae arcu. Donec iaculis venenatis eros, non convallis leo. Proin viverra consectetur ullamcorper. Praesent fringilla auctor consectetur. Donec vel leo mauris. Etiam tincidunt, libero eu efficitur ultrices, augue nisl suscipit lectus, id condimentum sapien justo nec orci. Mauris ultricies, tortor et auctor vestibulum, turpis enim sodales eros, lacinia blandit diam urna at metus. Aliquam risus libero, euismod vel lacinia sodales, dignissim ut libero. Nam at ligula risus. Aliquam rhoncus ullamcorper vestibulum. Ut sit amet mi nec dui aliquam congue ac vitae sapien. Proin hendrerit consequat posuere.\r\n\r\nVestibulum id tortor odio. Aliquam nec fermentum nulla. Quisque pharetra elit at sagittis cursus. Mauris rutrum justo id lectus mattis, et sagittis dolor mattis. Morbi pharetra massa non eros sagittis, feugiat malesuada metus tincidunt. Aliquam sodales, ligula porta mollis iaculis, nulla arcu volutpat ligula, quis vulputate nulla mi id purus. Praesent tincidunt magna non ex ullamcorper, sed egestas erat faucibus. Pellentesque vestibulum sit amet risus sit amet gravida. Nullam posuere diam ut ex egestas, et vestibulum purus sagittis. Sed vel magna non velit imperdiet congue. Nulla sit amet pulvinar arcu. Suspendisse in sapien suscipit, ultrices lectus at, ultrices quam. Vivamus mollis auctor ante id gravida.</p>";
                name = name.Replace("\r", "").Replace("\n", "");
            dataSource.DataSource = new Karte2Model()
            {
                Id = "PXP",
                tempObjs = new List<TempObj>() {
                new TempObj(){
                Class = "A1",
                Name = "PXP"
                },
                new TempObj(){
                Class = "A2",
                Name = name
                },
                  new TempObj(){
                Class = "A3",
                Name = "PXP3"
                },
                }
            };
            report.DataSource = dataSource;
            report.DataMember = "tempObjs";
            PdfExportOptions pdfExportOptions = new PdfExportOptions()
            {
                PdfACompatibility = PdfACompatibility.PdfA1b
            };

            // Specify the path for the exported PDF file.  
            string pdfExportFile =
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) +
                @"\Downloads\" +
                report.Name +
                ".pdf";

            // Export the report.
            report.ExportToPdf(pdfExportFile, pdfExportOptions);
        }
    }
}
