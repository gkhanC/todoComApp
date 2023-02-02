# TodoCom - A Command Line Todo App
___
TodoCom, PowerShell aracılığıyla görev yönetimi kolaylaştıran bir todo uygulamasıdır. Görev oluşturma, silme, kategorilere ayırma ve düzenleme fonksiyonları sunmaktadır.

## Özellikler
___

* Görev oluşturma
* Görev silme
* Görevlerin kategorilere ayrılması
* Daha önce oluşturulmuş görevlerin düzenlenmesi
* Görevlerin etiketlenmesi
* Görevlerin işaretlenmesi ve gruplandırılması (tamamlandı/tamamlanmadı)
* Görevlerin JSON formatında kaydedilmesi

## Kurulum
___
TodoCom'u kullanmak için aşağıdaki adımları izleyin:

1. Repoyu klonla veya indir.
2. Klonladığın dizine terminal ile gir ve aşağıdaki kodu çalıştır:

        dotnet build -c Release -o bin/Release

3. Derleme işlemi bittikten sonra bin/Release klasöründeki todoCOM.exe dosyasının yolunu kopyala.
4. Dosya yolunu aşağıdaki gibi düzenle:

        C:\Users\you\Documents\todoCOM\bin\Release\todoCOM.exe

5. PowerShell'i aç ve aşağıdaki kodu yaz
        
        notepad $PROFILE
        
Açılan dosya'ya aşağıdaki kodu düzenleyerek ekle, <b>'Path'</b> yazan yere todoCom.exe yolunu yapıştır :

        function todo {
                         cd $home
                         PATH $args
                       }

6. PowerShell'i kapat.
7. Kendi kullanıcı dizinine giderek Repository adında bir klasör oluştur.
8. PowerShell'i aç ve aşağıdaki kodu yaz:

           todo
9. Eğer benzer bir çıktı görürseniz, todoCom çalışıyor demektir.

        ______________________________________________________________________________________________________________
        TodoCOM Shows Message:
        TodoCom Initialized.
        TodoCom working on "main" category.
        ______________________________________________________________________________________________________________
        ______________________________________________________________________________________________________________


### Örnek Kullanım
___

Yeni bir görev oluşturmak için <b>'--add'</b> komutu kullanın ve boşluk bırakarak görev gövdesini yazın:

      todo --add "Yeni görev"


Görevleri görüntülemek için şu komutu kullanın:

      todo --show

Bir görevi tamamlamak için <b>'--complete'</b>, aşağıdaki komutu girin ve arade boşluk bırakıp görev Id'sini yazın:

      todo --complete 0

Görev silmek için aşağıdaki komutu girin ve görev Id'sini yazın:

      todo --delete 0


## Komutlar
___

### Add komutu => --add / -add:
* Temel işlevi yeni görev oluşturmaktır seçili kategori altında yeni görev oluşturur.
* Add komutundan sonra girilen veri oluşturulacak görevin gövdesini oluşturur.

Girdi:

        todoc --add do someting

veya

        todo -add "do someting"

Çıktı:

      ______________________________________________________________________________________________________________
      The Task added.
      ______________________________________________________________________________________________________________
      6    [_]  <do>:    do someting                                                   main        1.02.2023



* Add komutu bazı komutlarla birlikte kullanıla bilir.

#### Add ile birlikte kullanılan komutlar:

* --category ve ya -cat => girilen veriyi kategori olarak seçer ve görevi bu kategori altında oluşturur. Veri 10 karakterden kısa olmalıdır, bolşuk ve özel karakter içermemelidir.

Girdi:

      todo --add do someting --category new

Çıktı:

    ______________________________________________________________________________________________________________
    The Task added.
    ______________________________________________________________________________________________________________
    1    [_]  <do>:    do someting                                                   new         1.02.2023          


* --tag veya -tag => girilen veriyi oluşturulacak görevin etiketi olarak belirler. 3 veya daha kısa karakter uzunluğunda olmalı, boşluk ve özel karakter içermemlidir.

Girdi:

    todo --add buy new pen --category shopping --tag buy

Çıktı:

    ______________________________________________________________________________________________________________
    The Task added.
    ______________________________________________________________________________________________________________
    1    [_]  <buy>:   buy new pen                                                   shopping    1.02.2023


* --show ve ya -shw => görevi oluşturur ve kategori içerisindeki görevleri görüntüler. Tamamlanmış görevleri filtrelemek için <b>comp</b> deyimi ile birlikte kullanıma bilir => --show comp. Tamamlanmamış görevleri filtrelemek için  <b>uncomp</b> deyimi kullanılır.

Girdi:

    todo --add buy some paper --category shopping --show

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
* --category ve ya -cat => temel işlevi kategorileri yönetmektir.
* --category komutundan sonra bir deyim kullanılmaz ise seçili kategori ve todoCom içerisindeki diğer kategori bilgilerini görüntüler.

Girdi:

    todo --category

Çıktı:

    ______________________________________________________________________________________________________________
    TodoCOM Shows Message:                                                           
    Selected Category: main
    ______________________________________________________________________________________________________________

    id: 0 category name: main
    ______________________________________________________________________________________________________________


* --category integer tipinden bir deyim ile birlikte kullanılır ise deyimin karşılık geldiği id'deki kategoriyi seçer.


    todo --category 0


* --category komutu ile yeni kategory oluşturmak için komutdan sonra, string tipinde 10 karakterden kısa bir deyim girilir. --add komutundan sonra ve ya hariciyen kullanıla bilir.


    todo --category newcat

veya

    todo --category "newcat"


### Clean Komutu => --clean ve ya -clean

* todoCom içerisindeki kayıtlı tüm kategori ve görevleri siler.

### Complete Komutu => --complete ve ya -com

* --complete ve ya -com => temel işlevi görevi tamalandı olarak işaretlemektir.
* --complete komutu integer tipinde deyim alabilir. Deyim ile seçili kategori altında id bilgisi eşleşen görevi tamamlandı olarak işaretler.


        todo -com 0

* --complete komutu --add ile birlikte kullanılabilir. Deyim olarak <b>t</b> ve ya <b>true</b> eklenecek görevi tamamlandı olarak işaretler.


        todo --add new task --complete t

* --complete komutu --edit komutu ile beraber kullanıla bilir. Deyim olarak <b>t</b> ve ya <b>true</b> eklenecek görevi tamamlandı olarak işaretler. Deyim olarak <b>f</b> ve  ya <b>false</b> eklenecek görevi tamamlandı olarak işaretler.


        todo --edit 0 "new body" -com false


### Delete Komutu => --delete ve ya -del

* --delete ve ya -del => temel işlevi görevleri silmektir.
* --delete komutu integer tipinde deyim ile birlikte kullanılır. Deyim ile seçili kategori içerisinde eşleşen görevi siler


        todo --delete 0

### Delete Category Komutu => --delete-category ve ya -delc

* --delete-category ve ya -delc => temel işlevi deyim ile belirtilen bir kategoriyi ve o kategorideki görevleri silmektir.
* --delete-category integer tipinde deyimler ile kullanılabilir. Deyim bir kategori id'si ile eşleştiğinde ilgili kategori silinir.
* --delete-category string tipinde deyimler ile kullanıla bilir. Deyim bir kategori ismi ile eşleştiğinde kategori silinir.

### Edit Komutu => --edit ve ya -edt

* --edit ve ya -edt => temel işlevi bir görevi düzenlemektir.
* --edit komutu bir integer bir string tipinde iki deyime ihtiyaç duyar.
* --edit komutundan sonra integer tipinden bir deyim ile düzenlenecek görevin id'si belirtilir
* --edit komutu id deyiminden sonra string tipinde bir deyim alarak deyim ile alınan veriyi görevin body'sine yazar.


        todo --edit 0 "new body"

* --edit komutundan sonra --complete, --tag, --show komutları kullanıla bilir.

### Edit Category Komutu => --edit-category ve ya -edtc

* --edit-category ve ya -edtc => temel işlemi kategori ismini değiştirmektir.
* --edit-category komutu bir integer bir string tipinde iki deyim ile kullanılır.
* --edit-category komutu integer tipinde düzenlenmek istenilen kategorinin id bilgisine karşılık gelen deyim ile kullanılır.
* --eidt-category komutu id deyiminden sonra yeni kategori ismini belirten string tipinde bir deyim ile kullanılır.


        todo --edit 0 new

Yukardaki örnek <b>main</b> kategorisinin simine <b>new</b> olarak değiştirir.

Kategori id'lerini görmek için:

        todo -cat

### Show Komutu => --show ve ya -shw

* --show ve ya -shw komutu => temel işlevi seçili kategory içerisindeki görevleri görüntülemektir.
* --show komutu yalın olarak kullanılabilir.
* --show komutu tamamlanmış görevleri görüntülemek için <b>comp</b> deyimi ile kullanılır.


        todo --show comp

* --show komutu tamamlanmamış görevleri göstermek için <b>uncomp</b> deyimi ile birlikte bullanılır.


        todo --show uncomp

* --show komutu --category komutu ile birlikte kullanılabilir.


        todo -cat 0 -shw

### Show All Komutu => --show-all ve ya -shwa

* --show-all ve ya -shwa => temel işlevi todoCom içerisindeki tüm taskleri görüntülemektir.


        todo -shwa

### Tag Komutu => --tag ve ya -tag

* --tag ve ya -tag komutu =>temel işlevi görevlerin etiketlerini düzenlemektir. Default değer <b>do</b> 'dır.
* --tag komutu --edit ve ya --add komutları ile kullanıla bilir.


        todo --add "do someting" --tag "do" --category "new"
