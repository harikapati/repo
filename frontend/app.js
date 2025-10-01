const API_BASE = "http://localhost:5000";
  const { useState, useEffect } = React;
  const accountTypes = ["Checking", "Savings"];
  const accountTypeMap = { "Checking": 0, "Savings": 1 };

  function App() {
    const [balances, setBalances] = useState({ Checking: 0, Savings: 0 });
    const [amount, setAmount] = useState(0); // For deposit/withdraw
    const [transferAmount, setTransferAmount] = useState(0); // For transfer
    const [depositWithdrawAccount, setDepositWithdrawAccount] = useState("Checking");
    const [transferFrom, setTransferFrom] = useState("Checking");
    const [transferTo, setTransferTo] = useState("Savings");
    const [depositWithdrawMessage, setDepositWithdrawMessage] = useState("");
    const [transferMessage, setTransferMessage] = useState("");
    const [transactions, setTransactions] = useState([]);

    const fetchBalances = () => {
      accountTypes.forEach(type => {
        axios.get(`${API_BASE}/api/account/balance/${type}`)
          .then(res => {
            setBalances(b => ({ ...b, [type]: res.data }));
          });
      });
    };
    const fetchTransactions = () => {
      axios.get(`${API_BASE}/api/account/transactions`)
        .then(res => setTransactions(res.data));
    };

    useEffect(() => {
      fetchBalances();
      fetchTransactions();
    }, []);

    const MAX_AMOUNT = 100000;
    const handleDeposit = () => {
      if (!amount || Number(amount) < 1) {
        setDepositWithdrawMessage("Amount must be at least 1.");
        return;
      }
      if (Number(amount) > MAX_AMOUNT) {
        setDepositWithdrawMessage(`Amount must not exceed $${MAX_AMOUNT}.`);
        return;
      }
      axios.post(`${API_BASE}/api/account/deposit`, { type: accountTypeMap[depositWithdrawAccount], amount: Number(amount) })
        .then(() => {
          setDepositWithdrawMessage("Deposit successful");
          setAmount("");
          fetchBalances();
          fetchTransactions();
        })
        .catch(e => {
          const err = e.response?.data;
          setDepositWithdrawMessage(typeof err === 'string' ? err : JSON.stringify(err));
        });
    };

    const handleWithdraw = () => {
      if (!amount || Number(amount) < 1) {
        setDepositWithdrawMessage("Amount must be at least 1.");
        return;
      }
      if (Number(amount) > MAX_AMOUNT) {
        setDepositWithdrawMessage(`Amount must not exceed $${MAX_AMOUNT}.`);
        return;
      }
      axios.post(`${API_BASE}/api/account/withdraw`, { type: accountTypeMap[depositWithdrawAccount], amount: Number(amount) })
        .then(() => {
          setDepositWithdrawMessage("Withdraw successful");
          setAmount("");
          fetchBalances();
          fetchTransactions();
        })
        .catch(e => {
          const err = e.response?.data;
          setDepositWithdrawMessage(typeof err === 'string' ? err : JSON.stringify(err));
        });
    };

    const handleTransfer = () => {
      if (!transferAmount || Number(transferAmount) < 1) {
        setTransferMessage("Amount must be at least 1.");
        return;
      }
      if (Number(transferAmount) > MAX_AMOUNT) {
        setTransferMessage(`Amount must not exceed $${MAX_AMOUNT}.`);
        return;
      }
      axios.post(`${API_BASE}/api/account/transfer`, { from: accountTypeMap[transferFrom], to: accountTypeMap[transferTo], amount: Number(transferAmount) })
        .then(() => {
          setTransferMessage("Transfer successful");
          setTransferAmount("");
          fetchBalances();
          fetchTransactions();
        })
        .catch(e => {
          const err = e.response?.data;
          setTransferMessage(typeof err === 'string' ? err : JSON.stringify(err));
        });
    };

    return (
  React.createElement("div", { style: { maxWidth: 600, margin: "auto" } },
        React.createElement("h2", null, "ATM Application"),
        React.createElement("div", null,
          React.createElement("h4", null, "Balances"),
          React.createElement("p", null, `Checking: $${balances.Checking}`),
          React.createElement("p", null, `Savings: $${balances.Savings}`)
        ),
        React.createElement("div", null,
          React.createElement("input", {
            type: "number",
            min: 1,
            value: amount,
            onChange: e => {
              const val = e.target.value;
              if (val === "" || (/^\d+$/.test(val) && Number(val) >= 1)) {
                setAmount(val.replace(/^0+/, ''));
              }
              if (depositWithdrawMessage) setDepositWithdrawMessage("");
              if (transferMessage) setTransferMessage("");
            },
            placeholder: "Amount"
          }),
          React.createElement("select", {
            value: depositWithdrawAccount,
            onChange: e => setDepositWithdrawAccount(e.target.value)
          },
            accountTypes.map(type => React.createElement("option", { key: type }, type))
          ),
          React.createElement("button", {
            onClick: handleDeposit,
            disabled: !amount || Number(amount) <= 0
          }, "Deposit"),
          React.createElement("button", {
            onClick: handleWithdraw,
            disabled: !amount || Number(amount) <= 0
          }, "Withdraw"),
          React.createElement("div", { style: { color: "red" } }, depositWithdrawMessage)
        ),
        React.createElement("div", null,
          React.createElement("h4", null, "Transfer"),
          React.createElement("select", {
            value: transferFrom,
            onChange: e => {
              setTransferFrom(e.target.value);
              if (e.target.value === transferTo) {
                setTransferTo(accountTypes.find(t => t !== e.target.value));
              }
            }
          },
            accountTypes.map(type => React.createElement("option", { key: type }, type))
          ),
          React.createElement("select", {
            value: transferTo,
            onChange: e => setTransferTo(e.target.value),
          },
            accountTypes.map(type => React.createElement("option", { key: type, disabled: type === transferFrom }, type))
          ),
          React.createElement("input", {
            type: "number",
            min: 1,
            value: transferAmount,
            onChange: e => {
              const val = e.target.value;
              if (val === "" || (/^\d+$/.test(val) && Number(val) >= 1)) {
                setTransferAmount(val.replace(/^0+/, ''));
              }
              if (transferMessage) setTransferMessage("");
              if (depositWithdrawMessage) setDepositWithdrawMessage("");
            },
            placeholder: "Amount"
          }),
          React.createElement("button", {
            onClick: handleTransfer,
            disabled: !transferAmount || Number(transferAmount) <= 0
          }, "Transfer"),
          React.createElement("div", { style: { color: "red" } }, transferMessage),
          React.createElement("div", { style: { marginTop: 32 } },
            React.createElement("h4", null, "Transaction History"),
            transactions.length === 0
              ? React.createElement("p", null, "No transactions yet.")
              : React.createElement("table", { border: 1, cellPadding: 6, style: { width: "100%", marginTop: 8 } },
                  React.createElement("thead", null,
                    React.createElement("tr", null,
                      React.createElement("th", null, "Date"),
                      React.createElement("th", null, "Description"),
                      React.createElement("th", null, "Amount"),
                      React.createElement("th", null, "Account")
                    )
                  ),
                  React.createElement("tbody", null,
                    [...transactions].reverse().map(tx => React.createElement("tr", { key: tx.id },
                      React.createElement("td", null, new Date(tx.date).toLocaleString()),
                      React.createElement("td", null, tx.description),
                      React.createElement("td", null, `$${tx.amount}`),
                      React.createElement("td", null, tx.accountType === 0 ? "Checking" : "Savings")
                    ))
                  )
                )
          )
        ),
  // ...existing code...
      )
    );
  }

  ReactDOM.render(React.createElement(App), document.getElementById("root"));
