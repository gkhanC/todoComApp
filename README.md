# TodoCom - A Command Line Todo App
TodoCom, komut istemi aracılığıyla görev yönetmenizi kolaylaştıran bir todo uygulamasıdır. Görev oluşturma, silme, kategorilere ayırma ve düzenleme fonksiyonları sunar.
___

## Özellikler
___
* Görev oluşturma
* Görev silme
* Görevleri kategorilere ayırma
* Daha önce oluşturulmuş görevleri düzenleme
* Görevler, etiketleme
* Görevleri işaretleme ve gruplama ( tamamlandı / tamamlanmadı )
* Görevleri JSON formatında kaydetme

## Kurulum
___
TodoCom'u kullanmak için aşağıdaki adımları izleyin:

Repository'yi klonlayın veya indirin.
Klonladığınız dizine terminal ile girin ve aşağıdaki kodu çalıştırın.

        dotnet build -c Release -o bin/Release

Derleme işlemi bittikten sonra bin/Release klasörü içerisindeki todoCOm.exe dosyasının yolunu kopyalayın.

Dosya yolu aşağıdaki gibi görünmeli:

        C:\Users\you\Documents\todoCOM\bin\Release


Windows Terminal ve ya PowerShell'i açın ve aşağıdaki kodu yazınız. Çıktıda görünen path'e gidip profile dosyasını açınız
    
        $profile

Bilgisayarınızda VsCode yüklü ise aşağıdaki kod ile bu işlemi yapabilirsiniz.

        code $profile


Açılan dosyaya aşağıdaki satırı ekleyip ve kopyaladığımız dosya yolunu içerekcek şeklde satırı düzenleyip kaydedin.

        function todocom { Kopyaladığımız dosya youlu buraya yapıştırılacak son kısmı silmeyin\todoCOM.exe}

Satır şu şekilde görünmeli:

        function todocom {  C:\Users\you\Documents\todoCOM\bin\Release\todoCOM.exe}


PowerShell veya windows terminal'i kapatın.

Kendi kullanıcı dizinize giderek Repository adında bir klasör oluşturun.
Kullanıcı dizininiz şu şekilde görünmeli:

        C:\Users\yourusername

PowerShell ve ya Windows Terminali açın ve aşağıdaki kodu yazın.

        todocom

Ekranda şuan benzer bir çıktı görürseniz todoCom çalışıyor demektir.

        ______________________________________________________________________________________________________________
        TodoCOM Shows Message:
        TodoCom Initialized.
        TodoCom working on "main" category.
        ______________________________________________________________________________________________________________
        ______________________________________________________________________________________________________________


### Kullanım
___
Yeni bir görev oluşturmak için --add komutunu kullanın ve boşluk bırakıp görev gövdesini yazınız kullanın.
        
        todocom --add "Yeni görev"

Görevleri görüntülemek için --show komutunu kullanın.

        todocom --show

bir görevi tamamlamak için görev --complete komutundan sonra görev Id'sini giriniz.

        todocom --complete 0

Görev silmek için --delete komutundan sonra görev Id'sini giriniz.

        todocom --delete 0

Daha ayrıntılı kullanım talimatları için Komutlar Başlığına göz atınız.

## Komutlar
___

### Add komutu => --add / -add:
* Temel işlevi yeni görev oluşturmaktır Seçili kategori altında geni görev oluşturur.
* Add komutundan sonra girilen veri oluşturulacak görevin gövdesini oluşturur.

Girdi: 

        todocom --add do someting

veya

        todocom --add "do someting"

Çıktı:

      ______________________________________________________________________________________________________________
      The Task added.
      ______________________________________________________________________________________________________________
      6    [_]  <do>:    do someting                                                   main        1.02.2023



* Add komutu bazı komutlarla birlikte kullanıla bilir.

#### Add ile birlikte kullanılan komutlar:

* --category veya -cat => girilen veriyi kategori olarak seçer ve görevi bu kategori altında oluşturur. Veri 10 karakterden kısa olmalıdır, bolşuk ve özel karakter içermemelidir.

Girdi: 

      todocom --add do someting --category new

Çıktı:

    ______________________________________________________________________________________________________________
    The Task added.
    ______________________________________________________________________________________________________________
    1    [_]  <do>:    do someting                                                   new         1.02.2023          


* --tag veya -tag => girilen veriyi oluşturulacak görevin etiketi olarak belirler. 3 veya daha kısa karakter uzunluğunda olmalı, boşluk ve özel karakter içermemlidir.

Girdi: 

    todocom --add buy new pen --category shopping --tag buy

Çıktı:

    ______________________________________________________________________________________________________________
    The Task added.
    ______________________________________________________________________________________________________________
    1    [_]  <buy>:   buy new pen                                                   shopping    1.02.2023


* --show veya -shw => görevi oluşturur ve kategori içerisindeki görevleri görüntüler. Tamamlanmış görevleri filtrelemek için <b>comp</b> deyimi ile birlikte kullanıma bilir => --show comp. Tamamlanmamış görevleri filtrelemek için  <b>uncomp</b> deyimi kullanılır.

Girdi:

    todocom  --add buy some paper --category shopping --show

Çıktı:

    ______________________________________________________________________________________________________________
    The Task added.
    ______________________________________________________________________________________________________________
    2    [_]  <do>:    buy some paper                                                shopping    1.02.2023

    _________________________________________________________________________________________________________________
    UnCompleted Tasks: 2     Completed Tasks: 0                                        Total number of tasks:2     
    TodoCOM Shows All Tasks in "Category: shopping"
    
    Id   Done <Tag>:   Title                                                         Category    CreateDate   DueDate
    _________________________________________________________________________________________________________________
    1    [_]  <buy>:   buy new pen                                                   shopping    1.02.2023
    
    2    [_]  <do>:    buy some paper                                                shopping    1.02.2023          


### Kategori komutu => --category veya -cat 

* Default değeri main'dir. "main" todoCom'un kullandığı temek kategoridir.
* --category veya -cat => temel işlevi kategorileri yönetmektir.
* --category komutundan sonra bir deyim kullanılmaz ise seçili kategori ve todoCom içerisindeki diğer kategori bilgilerini görüntüler.

Girdi:

    todocom --category

Çıktı:

    ______________________________________________________________________________________________________________
    TodoCOM Shows Message:                                                           
    Selected Category: main
    ______________________________________________________________________________________________________________

    id: 0 category name: main
    ______________________________________________________________________________________________________________


* --category integer tipinden bir deyim ile birlikte kullanılır ise deyimin karşılık geldiği id'deki kategoriyi seçer.


    todocom --category 0


* --category komutu ile yeni kategory oluşturmak için komutdan sonra, string tipinde 10 karakterden kısa bir deyim girilir. --add komutundan sonra veya hariciyen kullanıla bilir.


    todocom --category newcat

veya

    todocom --category "newcat"


### Clean Komutu => --clean ve ya -clean

* todoCom içerisindeki kayıtlı tüm kategori ve görevleri siler.

### Complete Komutu => --complete ve ya -com

* --complete veya -com => temel işlevi görevi tamalandı olarak işaretlemektir.
* --complete komutu integer tipinde deyim alabilir. Deyim ile seçili kategori altında id bilgisi eşleşen görevi tamamlandı olarak işaretler.

    
    todocom -com 0

* --complete komutu --add ile birlikte kullanılabilir. Deyim olarak <b>t</b> veya <b>true</b> eklenecek görevi tamamlandı olarak işaretler.

    
    todocom --add new task --complete t

* --complete komutu --edit komutu ile beraber kullanıla bilir. Deyim olarak <b>t</b> veya <b>true</b> eklenecek görevi tamamlandı olarak işaretler. Deyim olarak <b>f</b> veya <b>false</b> eklenecek görevi tamamlandı olarak işaretler.

    
    todocom --edit 0 "new body" -com false


### Delete Komutu => --delete ve ya -del

* --delete ve ya -del => temel işlevi görevleri silmektir.
* --delete komutu integer tipinde deyim ile birlikte kullanılır. Deyim ile seçili kategori içerisinde eşleşen görevi siler

    
    todocom --delete 0

### Delete Category Komutu => --delete-category ve ya -delc

* --delete-category ve ya -delc => temel işlevi deyim ile belirtilen bir kategoriyi ve o kategorideki görevleri silmektir.
* --delete-category integer tipinde deyimler ile kullanılabilir. Deyim bir kategori id'si ile eşleştiğinde ilgili kategori silinir.
* --delete-category string tipinde deyimler ile kullanıla bilir. Deyim bir kategori ismi ile eşleştiğinde kategori silinir.

### Edit Komutu => --edit ve ya -edt 

* --edit ve ya -edt => temel işlevi bir görevi düzenlemektir.
* --edit komutu bir integer bir string tipinde iki deyime ihtiyaç duyar.
* --edit komutundan sonra integer tipinden bir deyim ile düzenlenecek görevin id'si belirtilir 
* --edit komutu id deyiminden sonra string tipinde bir deyim alarak deyim ile alınan veriyi görevin body'sine yazar.

    
    todocom --edit 0 "new body"

* --edit komutundan sonra --complete, --tag, --show komutları kullanıla bilir.

### Edit Category Komutu => --edit-category ve ya -edtc

* --edit-category ve ya -edtc => temel işlemi kategori ismini değiştirmektir.
* --edit-category komutu bir integer bir string tipinde iki deyim ile kullanılır.
* --edit-category komutu integer tipinde düzenlenmek istenilen kategorinin id bilgisine karşılık gelen deyim ile kullanılır.
* --eidt-category komutu id deyiminden sonra yeni kategori ismini belirten string tipinde bir deyim ile kullanılır.


    todocom --edit 0 new

Yukardaki örnek <b>main</b> kategorisinin simine <b>new</b> olarak değiştirir.

Kategori id'lerini görmek için:

    todocom -cat

### Show Komutu => --show ve ya -shw

* --show ve ya -shw komutu => temel işlevi seçili kategory içerisindeki görevleri görüntülemektir.
* --show komutu yalın olarak kullanılabilir.
* --show komutu tamamlanmış görevleri görüntülemek için <b>comp</b> deyimi ile kullanılır.

    
    todocom --show comp

* --show komutu tamamlanmamış görevleri göstermek için <b>uncomp</b> deyimi ile birlikte bullanılır.


    todocom --show uncomp

* --show komutu --category komutu ile birlikte kullanılabilir.


    todocom -cat 0 -shw

### Show All Komutu => --show-all ve ya -shwa

* --show-all ve ya -shwa => temel işlevi todoCom içerisindeki tüm taskleri görüntülemektir.


        todocom -shwa

### Tag Komutu => --tag ve ya -tag

* --tag ve ya -tag komutu =>temel işlevi görevlerin etiketlerini düzenlemektir. Default değer <b>do</b> 'dır.
* --tag komutu --edit ve ya --add komutları ile kullanıla bilir.

    
    todocom --add "do someting" --tag "do" --category "new"
