using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Pra.Bibliotheek.Core.Entities;
using Pra.Bibliotheek.Core.Services;
using Pra.Bibliotheek.Core.Interfaces;

namespace Pra.Bibliotheek.Wpf
{

    public partial class MainWindow : Window
    {
        IBookService bibService = new ConnectedService();
        bool isNew;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void RdbDisconnected_Checked(object sender, RoutedEventArgs e)
        {
            bibService = new DisconnectedService();
            Window_Loaded(null, null);
        }

        private void RdbConnected1_Checked(object sender, RoutedEventArgs e)
        {
            bibService = new ConnectedService();
            Window_Loaded(null, null);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PopulateBooks();
            PopulateAuthors();
            PopulatePublishers();
            ActivateLeft();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(bibService is DisconnectedService disconnectedService)
            {
                if (!disconnectedService.SaveData())
                {
                    MessageBox.Show("We konden de data niet bewaren.  Probeer nogmaals af te sluiten.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    e.Cancel = true;
                }
            }
        }

        private void PopulateBooks()
        {
            ClearControls();
            lstBooks.ItemsSource = null;
            Author author = (Author)cmbFilterAuthor.SelectedItem;
            Publisher publisher = (Publisher)cmbFilterPublisher.SelectedItem;
            lstBooks.ItemsSource = bibService.GetBooks(author, publisher);
        }
        private void PopulateAuthors()
        {
            cmbFilterAuthor.ItemsSource = null;
            cmbAuthor.ItemsSource = null;
            lstAuthors.ItemsSource = null;

            List<Author> authors = bibService.GetAuthors();

            cmbAuthor.SelectedValuePath = "ID";
            cmbAuthor.DisplayMemberPath = "Name";

            cmbFilterAuthor.ItemsSource = authors;
            cmbAuthor.ItemsSource = authors;
            lstAuthors.ItemsSource = authors;
        }
        private void PopulatePublishers()
        {
            cmbFilterPublisher.ItemsSource = null;
            cmbPublisher.ItemsSource = null;
            lstPublishers.ItemsSource = null;

            List<Publisher> publishers = bibService.GetPublishers();

            cmbPublisher.SelectedValuePath = "ID";
            cmbPublisher.DisplayMemberPath = "Name";

            cmbFilterPublisher.ItemsSource = publishers;
            cmbPublisher.ItemsSource = publishers;
            lstPublishers.ItemsSource = publishers;

        }
        private void ClearControls()
        {
            txtTitle.Text = "";
            txtYear.Text = "";
            cmbAuthor.SelectedIndex = -1;
            cmbPublisher.SelectedIndex = -1;
        }
        private void ActivateLeft()
        {
            grpLeft.IsEnabled = true;
            grpRight.IsEnabled = false;
            btnSave.Visibility = Visibility.Hidden;
            btnCancel.Visibility = Visibility.Hidden;

            grpAuteurs.IsEnabled = true;
            grpUitgevers.IsEnabled = true;
        }
        private void ActivateRight()
        {
            grpLeft.IsEnabled = false;
            grpRight.IsEnabled = true;
            btnSave.Visibility = Visibility.Visible;
            btnCancel.Visibility = Visibility.Visible;

            grpAuteurs.IsEnabled = false;
            grpUitgevers.IsEnabled = false;
        }

        private void CmbFilterAuthor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PopulateBooks();
        }
        private void CmbFilterPublisher_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PopulateBooks();
        }
        private void BtnClearFilterAuthor_Click(object sender, RoutedEventArgs e)
        {
            cmbFilterAuthor.SelectedIndex = -1;
            PopulateBooks();
        }
        private void BtnClearFilterPublisher_Click(object sender, RoutedEventArgs e)
        {
            cmbFilterPublisher.SelectedIndex = -1;
            PopulateBooks();
        }

        private void LstBooks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstBooks.SelectedItem != null)
            {
                Book book = (Book)lstBooks.SelectedItem;
                txtTitle.Text = book.Title;
                txtYear.Text = book.Year.ToString();
                cmbAuthor.SelectedValue = book.AuthorID;
                cmbPublisher.SelectedValue = book.PublisherID;
            }
        }

        private void BtnNew_Click(object sender, RoutedEventArgs e)
        {
            isNew = true;
            ActivateRight();
            ClearControls();
            txtTitle.Focus();
        }
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (lstBooks.SelectedItem != null)
            {
                isNew = false;
                ActivateRight();
                txtTitle.Focus();
            }
        }
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            ActivateLeft();
            LstBooks_SelectionChanged(null, null);
        }
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lstBooks.SelectedItem != null)
            {
                if (MessageBox.Show("Ben je zeker?", "Boek wissen", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Book book = (Book)lstBooks.SelectedItem;
                    if (!bibService.DeleteBook(book))
                    {
                        MessageBox.Show("We konden het boek niet verwijderen !", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    ClearControls();
                    PopulateBooks();
                }
            }
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            string title = txtTitle.Text.Trim();
            if (title.Length == 0)
            {
                MessageBox.Show("Je dient een titel op te geven !", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                txtTitle.Focus();
                return;
            }
            if (cmbAuthor.SelectedItem == null)
            {
                MessageBox.Show("Je dient een auteur te selecteren !", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                cmbAuthor.Focus();
                return;
            }
            Author author = (Author)cmbAuthor.SelectedItem;
            if (cmbPublisher.SelectedItem == null)
            {
                MessageBox.Show("Je dient een uitgever te selecteren !", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                cmbPublisher.Focus();
                return;
            }
            Publisher publisher = (Publisher)cmbPublisher.SelectedItem;
            int.TryParse(txtYear.Text, out int year);
            
            Book book;
            if (isNew)
            {
                book = new Book(title, author.ID, publisher.ID, year);
                if (!bibService.AddBook(book))
                {
                    MessageBox.Show("We konden het nieuwe boek niet bewaren.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            else
            {
                book = (Book)lstBooks.SelectedItem;
                book.Title = title;
                book.AuthorID = author.ID;
                book.PublisherID = publisher.ID;
                book.Year = year;
                if (!bibService.UpdateBook(book))
                {
                    MessageBox.Show("We konden het boek niet wijzigen.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            PopulateBooks();
            lstBooks.SelectedItem = book;
            LstBooks_SelectionChanged(null, null);
            ActivateLeft();
        }

        private void LstAuthors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtAuthor.Text = "";
            if (lstAuthors.SelectedItem != null)
            {
                txtAuthor.Text = ((Author)lstAuthors.SelectedItem).Name;
            }
        }
        private void BtnClearAuthor_Click(object sender, RoutedEventArgs e)
        {
            lstAuthors.SelectedIndex = -1;
            txtAuthor.Text = "";
            txtAuthor.Focus();
        }
        private void BtnAddAuthor_Click(object sender, RoutedEventArgs e)
        {
            string name = txtAuthor.Text.Trim();
            if (name.Length == 0)
            {
                MessageBox.Show("Je dient een naam op te geven !", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                txtAuthor.Focus();
                return;
            }
            Author author = new Author(name);
            if (!bibService.AddAuthor(author))
            {
                MessageBox.Show("We konden de nieuwe auteur niet bewaren !", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                txtAuthor.Focus();
                return;
            }
            PopulateAuthors();
            lstAuthors.SelectedItem = author;
            LstAuthors_SelectionChanged(null, null);
        }
        private void BtnEditAuthor_Click(object sender, RoutedEventArgs e)
        {
            if (lstAuthors.SelectedItem == null)
            {
                MessageBox.Show("Je dient eerst een auteur te selecteren !", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string name = txtAuthor.Text.Trim();
            if (name.Length == 0)
            {
                MessageBox.Show("Je dient een naam op te geven !", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                txtAuthor.Focus();
                return;
            }
            Author author = (Author)lstAuthors.SelectedItem;
            author.Name = name;
            if (!bibService.UpdateAuthor(author))
            {
                MessageBox.Show("We konden de auteur niet wijzigen !", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                txtAuthor.Focus();
                return;
            }
            PopulateAuthors();
            lstAuthors.SelectedItem = author;
            LstAuthors_SelectionChanged(null, null);
        }
        private void BtnDeleteAuthor_Click(object sender, RoutedEventArgs e)
        {
            if (lstAuthors.SelectedItem == null)
            {
                MessageBox.Show("Je dient eerst een auteur te selecteren !", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Author author = (Author)lstAuthors.SelectedItem;
            if (bibService.IsAuthorInUse(author))
            {
                MessageBox.Show("Deze auteur is nog in gebruik en kan momenteel niet verwijderd worden !", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!bibService.DeleteAuthor(author))
            {
                MessageBox.Show("We konden de auteur niet verwijderen !", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            PopulateAuthors();
            txtAuthor.Text = "";
            lstAuthors.SelectedIndex = -1;
        }

        private void LstPublishers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtPublisher.Text = "";
            if (lstPublishers.SelectedItem != null)
            {
                txtPublisher.Text = ((Publisher)lstPublishers.SelectedItem).Name;
            }
        }
        private void BtnClearUitgever_Click(object sender, RoutedEventArgs e)
        {
            lstPublishers.SelectedIndex = -1;
            txtPublisher.Text = "";
            txtPublisher.Focus();
        }
        private void BtnAddPublisher_Click(object sender, RoutedEventArgs e)
        {
            string name = txtPublisher.Text.Trim();
            if (name.Length == 0)
            {
                MessageBox.Show("Je dient een naam op te geven !", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                txtPublisher.Focus();
                return;
            }
            Publisher publisher = new Publisher(name);
            if (!bibService.AddPublisher(publisher))
            {
                MessageBox.Show("We konden de nieuwe uitgever niet toevoegen !", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                txtPublisher.Focus();
                return;
            }
            PopulatePublishers();
            lstPublishers.SelectedItem = publisher;
            LstPublishers_SelectionChanged(null, null);
        }
        private void BtnEditPublisher_Click(object sender, RoutedEventArgs e)
        {
            if (lstPublishers.SelectedItem == null)
            {
                MessageBox.Show("Je dient eerst een uitgever te selecteren !", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string name = txtPublisher.Text.Trim();
            if (name.Length == 0)
            {
                MessageBox.Show("Je dient een naam op te geven !", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                txtPublisher.Focus();
                return;
            }
            Publisher publisher = (Publisher)lstPublishers.SelectedItem;
            publisher.Name = name;
            if (!bibService.UpdatePublisher(publisher))
            {
                MessageBox.Show("We konden de uitgever niet wijzigen !", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                txtPublisher.Focus();
                return;
            }
            PopulatePublishers();
            lstPublishers.SelectedItem = publisher;
            LstPublishers_SelectionChanged(null, null);
        }
        private void BtnDeletePulisher_Click(object sender, RoutedEventArgs e)
        {
            if (lstPublishers.SelectedItem == null)
            {
                MessageBox.Show("Je dient eerst een uitgever te selecteren !", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Publisher publisher = (Publisher)lstPublishers.SelectedItem;
            if (bibService.IsPublisherInUse(publisher))
            {
                MessageBox.Show("Deze uitgever is nog in gebruik en kan momenteel niet verwijderd worden !", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!bibService.DeletePublisher(publisher))
            {
                MessageBox.Show("We konden de uitgever niet verwijderen !", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            PopulatePublishers();
            txtPublisher.Text = "";
            lstPublishers.SelectedIndex = -1;
        }


    }
}
