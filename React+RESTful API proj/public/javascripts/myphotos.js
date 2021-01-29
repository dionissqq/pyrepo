

fetch("/api/v1/me")
    .then(x => x.json())
    .then(x=>{
        console.log(x)
        return(x._id)
    })
    .then(idstr=>{
    console.log(idstr)
    Promise.all([
        fetch("/templates/photos.mst").then(x => x.text()),
        fetch("/api/v1/photos?owner="+idstr).then(x => x.json()),
        fetch("/api/v1/photos?page=2&owner="+idstr).then(x => x.json()),
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
    })
        async function search(){
            document.getElementById('pagenum').innerText=itemsData.pageNumber
            itemsData=itemsData.displayData
            let promise=await fetch("/api/v1/me").then(x => x.json())
            idstr=promise._id
            document.getElementById('page').innerText=1
            //let heading=document.getElementById('heading').innerText
            let tag=document.getElementById('search').value
            console.log(tag)
            Promise.all([
                fetch("/templates/photos.mst").then(x => x.text()),
                fetch("/api/v1/photos?searching="+tag+"&owner="+idstr).then(x => x.json()),
                fetch("/api/v1/photos?searching="+tag+"&page=2&owner="+idstr).then(x => x.json()),
            ])
                .then(([templateStr, itemsData,nextData]) => {
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
                        document.getElementById('heading').innerText="current photo list"
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

    async function next(){
        let promise=await fetch("/api/v1/me").then(x => x.json())
        idstr=promise._id
        let currentpage=document.getElementById('page').innerHTML

        let nextpage=parseInt(currentpage)+1;
        console.log(nextpage)
        let tag=document.getElementById('search').value
        console.log(tag)
        Promise.all([
            fetch("/templates/photos.mst").then(x => x.text()),
            fetch("/api/v1/photos?searching="+tag+"&page="+nextpage+"&owner="+idstr).then(x => x.json()),
            fetch("/api/v1/photos?searching="+tag+"&page="+(parseInt(nextpage)+1)+"&owner="+idstr).then(x => x.json()),
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
                    document.getElementById('heading').innerText="current photo list"
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
    async function prev(){
        let currentpage=document.getElementById('page').innerHTML
        let promise=await fetch("/api/v1/me").then(x => x.json())
        idstr=promise._id
        let nextpage=parseInt(currentpage)-1;
        console.log(nextpage)
        let tag=document.getElementById('search').value
        console.log(tag)
        Promise.all([
            fetch("/templates/photos.mst").then(x => x.text()),
            fetch("/api/v1/photos?searching="+tag+"&page="+nextpage+"&owner="+idstr).then(x => x.json()),
            fetch("/api/v1/photos?searching="+tag+"&page="+(parseInt(nextpage)+1)+"&owner="+idstr).then(x => x.json()),
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
                    document.getElementById('heading').innerText="current photo list"
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