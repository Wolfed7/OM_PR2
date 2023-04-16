import numpy as np
import matplotlib.pyplot as plt
import matplotlib.ticker as ticker

def rosen():
    xList = np.arange(-10, 10, 0.05)
    yList = np.arange(-10, 10, 0.05)
    X, Y = np.meshgrid(xList, yList)

    Z = 100 * (Y - X**2)**2 + (1 - X)**2

    return X, Y, Z

def quadratic():
    xList = np.arange(-10, 10, 0.05)
    yList = np.arange(-10, 10, 0.05)
    X, Y = np.meshgrid(xList, yList)

    Z = 100 * (Y - X)**2 + (1 - X)**2

    return X, Y, Z

def variant():
    xList = np.arange(-10, 10, 0.05)
    yList = np.arange(-10, 10, 0.05)
    X, Y = np.meshgrid(xList, yList)

    x1 = 1 + (X - 2)**2 + ((Y - 2) / 2)**2
    x2 = 1 + ((X - 2)/3)**2 + (Y - 3)**2
    Z  = -((3.0/x1) + (2.0/x2))

    return X, Y, Z


def testing(num):
    switch = {
        "1": rosen(),
        "2": quadratic(),
        "3": variant(),
    }
    return switch.get(num, "Invalid input")

def main():
    x = []
    y = []

    with open("coords.txt") as file:
        for line in file:
            xC, yC = line.split()
            x.append(xC)
            y.append(yC)

    _levels = np.arange(0, 500, 1)
    figure, axes = plt.subplots(1, 1)

    num = input("Enter the test: \n1) Rosenbrock \n2) Quadratic \n3) Function for 8 variant\n")

    X, Y, Z = testing(num)

    plt.xlim(-7, 7)
    plt.ylim(-7, 7)
    plt.xticks(np.arange(-7, 7, 1))
    plt.yticks(np.arange(-7, 7, 1))

    #axes.xaxis.set_major_locator(ticker.MultipleLocator(1))
    #axes.xaxis.set_minor_locator(ticker.MultipleLocator(1))
    #axes.yaxis.set_major_locator(ticker.MultipleLocator(1))
    #axes.yaxis.set_minor_locator(ticker.MultipleLocator(1))

    #formatter = ticker.FormatStrFormatter('%.2f')
    #axes.xaxis.set_major_formatter(formatter)

    plt.xlabel("x1")
    plt.ylabel("x2")

    _contourf = axes.contourf(X, Y, Z, levels=_levels, cmap='rainbow', extend='max')
    cs = _contourf
    cs.cmap.set_over('white', alpha = 0.2)
    cs.changed()

    axes.plot(x, y, '-o', markersize=5, color='blue')   
    axes.plot(x[0], y[0], 'o', markersize=4, color='black')
    axes.plot(x[-1], y[-1], 'o', markersize=5, color='red')

    figure.colorbar(_contourf, shrink=1)

    #plt.grid()
    #axes.set_aspect(1)
    # plt.savefig("graphics/test_s_1.png")
    plt.show()

if __name__ == "__main__":
    main()
