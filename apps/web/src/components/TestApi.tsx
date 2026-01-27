import { useState } from "react"

const TestApi = () => {
  
    const [result, setResult] = useState<string>("")
  
    const callApi = async () => {
        try {
            const res = await fetch("http://localhost:5274/api/v1/");
            const text = await res.text();
            setResult(text);
        } catch (err) {
            setResult("Error fetching data from API");
            console.error(err);
        }
    }

    return (
    <div>
        <h2>Test API</h2>
        <button onClick={callApi}>Call API</button>
        <p>{result}</p>
    </div>
  )
}

export default TestApi