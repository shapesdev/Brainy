using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopFactory  {

    public ShopController controller { get; private set; }
    public ShopModel model { get; private set; }
    public ShopView view { get; private set; }

    public void Load(ShopView view, List<ShopItem> items)
    {
        this.model = new ShopModel();
        this.view = view;
        this.controller = new ShopController(model, view, items);
    }
}
