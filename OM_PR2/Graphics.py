import numpy as np
import matplotlib.pyplot as plt
import matplotlib.ticker as ticker

def Rosenbrock():
    xList = np.arange(-10, 10, 0.05)
    yList = np.arange(-10, 10, 0.05)
    X, Y = np.meshgrid(xList, yList)

    Z = 100 * (Y - X**2)**2 + (1 - X)**2

    _levels = np.arange(0, 40, 4)
    
    return X, Y, Z, _levels

def Quadratic():
    xList = np.arange(-10, 10, 0.05)
    yList = np.arange(-10, 10, 0.05)
    X, Y = np.meshgrid(xList, yList)

    Z = 100 * (Y - X)**2 + (1 - X)**2
    
    _levels = np.arange(0, 2000, 100)
    
    return X, Y, Z, _levels

def Variant():
    xList = np.arange(-10, 10, 0.05)
    yList = np.arange(-10, 10, 0.05)
    X, Y = np.meshgrid(xList, yList)

    x1 = 1 + (X - 2)**2 + ((Y - 2) / 2)**2
    x2 = 1 + ((X - 2)/3)**2 + (Y - 3)**2
    Z  = ((3.0/x1) + (2.0/x2))

    _levels = np.arange(0, 5, 0.2)

    return X, Y, Z, _levels


def testing(num):
    switch ={
        "1": Rosenbrock(),
        "2": Quadratic(),
        "3": Variant()
    }
    return switch.get(num, "Invalid input")

def main():
    x = []
    y = []
    x2 = []
    y2 = []

    with open("cSim.txt") as file:
        for line in file:
            xC, yC = line.split()
            x.append(float(xC))
            y.append(float(yC))

    with open("cBr.txt") as file:
        for line in file:
            xC, yC = line.split()
            x2.append(float(xC))
            y2.append(float(yC))

    num = input("Enter the test: \n1) Rosenbrock \n2) Quadratic \n3) Function for 8 variant\n")
    X, Y, Z, _levels = testing(num)


    figure, axes = plt.subplots()
    
    plt.xlim(-10, 10)
    plt.ylim(-10, 10)
    plt.xticks(np.arange(-10, 10, 1))
    plt.yticks(np.arange(-10, 10, 1))
    plt.xlabel("x")
    plt.ylabel("y")
    
    _contourf = axes.contour(X, Y, Z, levels=_levels, cmap='rainbow')


    axes.plot(x, y, '-o', markersize=5, color='black')   
    axes.plot(x[0], y[0], 'o', markersize=5, color='blue')
    axes.plot(x[-1], y[-1], 'o', markersize=5, color='red')


    axes.plot(x2, y2, '->', markersize=5, color='blue')   
    axes.plot(x2[0], y2[0], '>', markersize=5, color='blue')
    axes.plot(x2[-1], y2[-1], '>', markersize=5, color='red')


    figure.colorbar(_contourf, shrink=1)

    #plt.savefig("graphics/BFGS_Rosenbrock.png")
    figure.set_figwidth(16)
    figure.set_figheight(9)
    plt.show()

if __name__ == "__main__":
    main()
