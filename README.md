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
Klonladığınız dizine terminal ile girin.
npm install komutunu çalıştırın.
TodoCom'u node index.js komutu ile çalıştırın.
Kullanım
Yeni bir görev oluşturmak için node index.js add komutunu kullanın.
Görev silmek için node index.js remove komutunu kullanın.
Görevleri kategorilere ayırmak için node index.js categorize komutunu kullanın.
Daha önce oluşturulmuş görevleri düzenlemek için node index.js edit komutunu kullanın.
Lisans
TodoCom MIT License ile lisanslanmıştır.

Sorunlar ve Destek
Eğer uygulamada bir sorunla karşılaşırsanız veya daha fazla destek isterseniz, Github sayfamızda iletişim bilgilerini kullanarak bizimle iletişime geçebilirsiniz.


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


* --show veya -shw => görevi oluşturur ve kategori içerisindeki görevleri görüntüler. Tamamlanmış görevleri filtrelemek için <b>comp<b/> deyimi ile birlikte kullanıma bilir => --show comp. Tamamlanmamış görevleri filtrelemek için  <b>uncomp<b/> deyimi kullanılır.

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
