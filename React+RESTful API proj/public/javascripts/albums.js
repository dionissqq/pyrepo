console.log("Loading Browser JS app.js");

Promise.all([
    fetch("/templates/albums.mst").then(x => x.text()),
    fetch("/api/v1/albums").then(x => x.json()),
    fetch("/api/v1/albums?page=2").then(x => x.json()),
])
    .then(([templateStr, itemsData,nextData]) => {
        document.getElementById('pagenum').innerText=itemsData.pageNumber
        itemsData=itemsData.displayData
        if(nextData){
            if(nextData.error=="no such page")
                document.getElementById('nextButton').setAttribute("disabled","") 
        }
        console.log('itemsData', itemsData);
        const dataObject = {items: itemsData};
        const renderedHtmlStr = Mustache.render(templateStr, dataObject);
        return renderedHtmlStr;
    })
    .then(htmlStr => {
        const appEl = document.getElementById('parentElement');
        appEl.innerHTML = htmlStr;
    })
    .catch(err => console.error(err));
    function search(){
        document.getElementById('page').innerText=1
        let tag=document.getElementById('search').value
        console.log(tag)
        Promise.all([
            fetch("/templates/albums.mst").then(x => x.text()),
            fetch("/api/v1/albums?searching="+tag).then(x => x.json()),
            fetch("/api/v1/albums?page=2&searching="+tag).then(x => x.json()),
        ])
            .then(([templateStr, itemsData,nextData]) => {
                document.getElementById('pagenum').innerText=itemsData.pageNumber
                itemsData=itemsData.displayData
                if(nextData){
                    if(nextData.error=="no such page")
                        document.getElementById('nextButton').setAttribute("disabled","") 
                }
                if (itemsData.length==0){
                    document.getElementById('heading').innerText="no results"
                }
                else{
                    if(tag!='')
                        document.getElementById('heading').innerText="results for '"+tag+"'"
                    else
                    document.getElementById('heading').innerText="current album list"
                }
                console.log('itemsData', itemsData);
                const dataObject = {items: itemsData};
                const renderedHtmlStr = Mustache.render(templateStr, dataObject);
                return renderedHtmlStr;
            })
            .then(htmlStr => {
                const appEl = document.getElementById('parentElement');
                appEl.innerHTML = htmlStr;
            })
            .catch(err => console.error(err));
    }
    function next(){
        let currentpage=document.getElementById('page').innerHTML

        let nextpage=parseInt(currentpage)+1;
        console.log(nextpage)
        let tag=document.getElementById('search').value
        console.log(tag)
        Promise.all([
            fetch("/templates/albums.mst").then(x => x.text()),
            fetch("/api/v1/albums?searching="+tag+"&page="+nextpage).then(x => x.json()),
            fetch("/api/v1/albums?searching="+tag+"&page="+(parseInt(nextpage)+1)).then(x => x.json()),
        ])
            .then(([templateStr, itemsData,nextData]) => {
                document.getElementById('pagenum').innerText=itemsData.pageNumber
                itemsData=itemsData.displayData
                document.getElementById('page').innerText=nextpage
                if(nextpage>1)
                    document.getElementById('prevButton').removeAttribute("disabled")
                else
                    document.getElementById('prevButton').setAttribute("disabled","")
                document.getElementById('nextButton').removeAttribute("disabled")
                console.log(nextData)
                if(nextData){
                    if(nextData.error=="no such page")
                        document.getElementById('nextButton').setAttribute("disabled","") 
                }
                if (itemsData.length==0){
                    document.getElementById('heading').innerText="no results"
                }
                else{
                    if(tag!='')
                        document.getElementById('heading').innerText="results for '"+tag+"'"
                    else
                    document.getElementById('heading').innerText="current album list"
                }
                console.log('itemsData', itemsData);
                const dataObject = {items: itemsData};
                const renderedHtmlStr = Mustache.render(templateStr, dataObject);
                return renderedHtmlStr;
            })
            .then(htmlStr => {
                const appEl = document.getElementById('parentElement');
                appEl.innerHTML = htmlStr;
            })
            .catch(err => console.error(err));
    }
    function prev(){
        let currentpage=document.getElementById('page').innerHTML

        let nextpage=parseInt(currentpage)-1;
        console.log(nextpage)
        let tag=document.getElementById('search').value
        console.log(tag)
        Promise.all([
            fetch("/templates/albums.mst").then(x => x.text()),
            fetch("/api/v1/albums?searching="+tag+"&page="+nextpage).then(x => x.json()),
            fetch("/api/v1/albums?searching="+tag+"&page="+(parseInt(nextpage)+1)).then(x => x.json()),
        ])
            .then(([templateStr, itemsData,nextData]) => {
                document.getElementById('pagenum').innerText=itemsData.pageNumber
                itemsData=itemsData.displayData
                document.getElementById('page').innerText=nextpage
                if(nextpage>1)
                    document.getElementById('prevButton').removeAttribute("disabled")
                else
                    document.getElementById('prevButton').setAttribute("disabled","")
                document.getElementById('nextButton').removeAttribute("disabled")
                console.log(nextData)
 
                    if(nextData.error=="no such page")
                        document.getElementById('nextButton').setAttribute("disabled","") 

                if (itemsData.length==0){
                    document.getElementById('heading').innerText="no results"
                }
                else{
                    if(tag!='')
                        document.getElementById('heading').innerText="results for '"+tag+"'"
                    else
                    document.getElementById('heading').innerText="current album list"
                }
                console.log('itemsData', itemsData);
                const dataObject = {items: itemsData};
                const renderedHtmlStr = Mustache.render(templateStr, dataObject);
                return renderedHtmlStr;
            })
            .then(htmlStr => {
                const appEl = document.getElementById('parentElement');
                appEl.innerHTML = htmlStr;
            })
            .catch(err => console.error(err));
    }