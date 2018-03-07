using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Grid.Data;
using Grid.Entities.HRMS;
using Grid.Entities.Recruit;
using Grid.Entities.Recruit.Enums;
using Microsoft.Win32;

namespace FileImporter
{
    public partial class MainWindow
    {
        private readonly GridDataContext _db = DbContextFactory.Create();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog();
            var result = fileDialog.ShowDialog();
            if (result.GetValueOrDefault())
            {
                var fileName = fileDialog.FileName;
                var fileContents = File.ReadAllLines(fileName);
                foreach (var line in fileContents)
                {
                    try
                    {
                        var columns = line.Split(',');

                        var newPerson = new Person();

                        if (!string.IsNullOrEmpty(columns[0]))
                        {
                            var names = columns[0].Trim().Split(' ');
                            switch (names.Length)
                            {
                                case 1:
                                    newPerson.FirstName = names[0];
                                    break;
                                case 2:
                                    newPerson.FirstName = names[0];
                                    newPerson.LastName = names[1];
                                    break;
                                default:
                                    if (names.Length > 2)
                                    {
                                        newPerson.FirstName = names[0];
                                        newPerson.LastName = names[names.Length - 1];
                                        newPerson.MiddleName = string.Join(" ", names.ToArray(), 1, names.Length - 2);
                                    }
                                    break;
                            }
                        }

                        if (!string.IsNullOrEmpty(columns[1]))
                        {
                            newPerson.Designation = columns[1].Trim();
                        }

                        if (!string.IsNullOrEmpty(columns[2]))
                        {
                            newPerson.Email = columns[2].Trim().ToLower();
                        }

                       

                        _db.Persons.Add(newPerson);

                        var newCandidate = new Candidate
                        {
                            Source = CandidatesSource.Unknown,
                            Status = CandidateStatus.New,
                            PersonId = newPerson.Id,
                            CreatedByUserId = 1
                        };

                        _db.Candidates.Add(newCandidate);

                        _db.SaveChanges();
                    }
                    catch (Exception)
                    {
                        
                    }
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var assets = _db.Assets.ToList();
            foreach (var asset in assets)
            {
                asset.TagNumber = string.Format("LA{0}", asset.Id.ToString("D" + 6));
                _db.Entry(asset).State = EntityState.Modified;
                _db.SaveChanges();
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var candidates = _db.Candidates.Include("Person").ToList();
            foreach (var candidate in candidates)
            {
                // Clean up white spaces for Email and Phone Numbers
                if (!string.IsNullOrEmpty(candidate.Person.Email))
                {
                    candidate.Person.Email = candidate.Person.Email.Trim();
                }

                if (!string.IsNullOrEmpty(candidate.Person.SecondaryEmail))
                {
                    candidate.Person.SecondaryEmail = candidate.Person.SecondaryEmail.Trim();
                }

                if (!string.IsNullOrEmpty(candidate.Person.PhoneNo))
                {
                    candidate.Person.PhoneNo = candidate.Person.PhoneNo.Trim();
                }

                if (!string.IsNullOrEmpty(candidate.Person.OfficePhone))
                {
                    candidate.Person.OfficePhone = candidate.Person.OfficePhone.Trim();
                }

                _db.Entry(candidate).State = EntityState.Modified;
                _db.SaveChanges();
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var task = Task.Factory.StartNew(() =>
            {
                var candidates = _db.Candidates.Where(c => c.Id > 890).ToList();
                foreach (var candidate in candidates)
                {
                    candidate.Code = string.Format("LC{0}", candidate.Id.ToString("D" + 8));
                    _db.Entry(candidate).State = EntityState.Modified;
                    _db.SaveChanges();
                }
            });

            task.ContinueWith(y =>
            {
                MessageBox.Show("Update Complete");
            });
        }
    }
}
