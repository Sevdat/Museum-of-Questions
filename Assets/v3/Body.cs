using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Body : MonoBehaviour
{
    public HumanBody body = new HumanBody();
    public class HumanBody: WorldBuilder.bodyStructure{
        public void fun(){
            Index index0 = new Index(
                    0, 
                    new IndexConnections[]{
                        connections(1,7f)
                    }, 
                    new MeshStructure(){
                            drawCube = new Cube(){
                                frontSquare = distanceFromCenter(
                                    corner(-10,10,10),corner(10,10,10),
                                    corner(-10,-10,10),corner(10,-10,10)
                                    ),
                                backSquare = distanceFromCenter(
                                    corner(-10,10,-10),corner(10,10,-10),
                                    corner(-10,-10,-10),corner(10,-10,-10)
                                    )
                            },
                            deleteFromCube = new Cube[]{
                                new Cube(){
                                    frontSquare = distanceFromCenter(
                                        corner(-3,3,10),corner(3,3,10),
                                        corner(-3,-3,10),corner(3,-3,10)
                                        ),
                                    backSquare = distanceFromCenter(
                                        corner(-3,3,-10),corner(3,3,-10),
                                        corner(-3,-3,-10),corner(3,-3,-10)
                                            ),
                                    rotateCube = new RotateCube[]{
                                        rotateCube(
                                            30,
                                            corner(0,0,0),
                                            corner(1,0,0),
                                            true
                                            )
                                    }
                                }
                            }
                        }
                    );
            Index index1 = new Index(
                    1, 
                    new IndexConnections[]{
                        connections(2,2f)
                    }, 
                    new MeshStructure(){
                            drawCube = new Cube(){
                                frontSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    ),
                                backSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    )
                            },
                            deleteFromCube = new Cube[]{
                                new Cube(){
                                    frontSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        ),
                                    backSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        )
                                }
                            }
                        }
                    );
            Index index2 = new Index(
                    2, 
                    new IndexConnections[]{
                        connections(3,2f)
                    }, 
                    new MeshStructure(){
                            drawCube = new Cube(){
                                frontSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    ),
                                backSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    )
                            },
                            deleteFromCube = new Cube[]{
                                new Cube(){
                                    frontSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        ),
                                    backSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        )
                                }
                            }
                        }
                    );
            Index index3 = new Index(
                    3, 
                    new IndexConnections[]{
                        connections(4,6f),
                        connections(33,0f),
                        connections(34,0f)
                    }, 
                    new MeshStructure(){
                            drawCube = new Cube(){
                                frontSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    ),
                                backSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    )
                            },
                            deleteFromCube = new Cube[]{
                                new Cube(){
                                    frontSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        ),
                                    backSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        )
                                }
                            }
                        }
                    );
            Index index4 = new Index(
                    4, 
                    new IndexConnections[]{
                        connections(5,6f)
                    }, 
                    new MeshStructure(){
                            drawCube = new Cube(){
                                frontSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    ),
                                backSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    )
                            },
                            deleteFromCube = new Cube[]{
                                new Cube(){
                                    frontSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        ),
                                    backSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        )
                                }
                            }
                        }
                    );
            Index index5 = new Index(
                    5, 
                    new IndexConnections[]{
                        connections(6,4f)
                    }, 
                    new MeshStructure(){
                            drawCube = new Cube(){
                                frontSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    ),
                                backSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    )
                            },
                            deleteFromCube = new Cube[]{
                                new Cube(){
                                    frontSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        ),
                                    backSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        )
                                }
                            }
                        }
                    );
            Index index6 = new Index(
                    6, 
                    new IndexConnections[]{
                        connections(35,0f),
                        connections(36,0f)
                    }, 
                    new MeshStructure(){
                            drawCube = new Cube(){
                                frontSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    ),
                                backSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    )
                            },
                            deleteFromCube = new Cube[]{
                                new Cube(){
                                    frontSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        ),
                                    backSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        )
                                }
                            }
                        }
                        );
            Index index7 = new Index(
                    7, 
                    new IndexConnections[]{
                        connections(9,6f)
                    }, 
                    new MeshStructure(){
                            drawCube = new Cube(){
                                frontSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    ),
                                backSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    )
                            },
                            deleteFromCube = new Cube[]{
                                new Cube(){
                                    frontSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        ),
                                    backSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        )
                                }
                            }
                        }
                        );
            Index index8 = new Index(
                    8, 
                    new IndexConnections[]{
                        connections(10,6f)
                    }, 
                    new MeshStructure(){
                            drawCube = new Cube(){
                                frontSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    ),
                                backSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    )
                            },
                            deleteFromCube = new Cube[]{
                                new Cube(){
                                    frontSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        ),
                                    backSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        )
                                }
                            }
                        }
                    );
            Index index9 = new Index(
                    9, 
                    new IndexConnections[]{
                        connections(11,6f)
                    }, 
                    new MeshStructure(){
                            drawCube = new Cube(){
                                frontSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    ),
                                backSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    )
                            },
                            deleteFromCube = new Cube[]{
                                new Cube(){
                                    frontSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        ),
                                    backSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        )
                                }
                            }
                        }
                    );
            Index index10 = new Index(
                    10, 
                    new IndexConnections[]{
                        connections(12,6f)
                    }, 
                    new MeshStructure(){
                            drawCube = new Cube(){
                                frontSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    ),
                                backSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    )
                            },
                            deleteFromCube = new Cube[]{
                                new Cube(){
                                    frontSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        ),
                                    backSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        )
                                }
                            }
                        }
                    );
            Index index11 = new Index(
                    11, 
                    new IndexConnections[]{
                        connections(13,6f)
                    }, 
                    new MeshStructure(){
                            drawCube = new Cube(){
                                frontSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    ),
                                backSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    )
                            },
                            deleteFromCube = new Cube[]{
                                new Cube(){
                                    frontSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        ),
                                    backSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        )
                                }
                            }
                        }
                    );
            Index index12 = new Index(
                    12, 
                    new IndexConnections[]{
                        connections(14,6f)
                    }, 
                    new MeshStructure(){
                            drawCube = new Cube(){
                                frontSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    ),
                                backSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    )
                            },
                            deleteFromCube = new Cube[]{
                                new Cube(){
                                    frontSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        ),
                                    backSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        )
                                }
                            }
                        }
                    );
            Index index13 = new Index(
                    13, 
                    new IndexConnections[]{
                    }, 
                    new MeshStructure(){
                            drawCube = new Cube(){
                                frontSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    ),
                                backSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    )
                            },
                            deleteFromCube = new Cube[]{
                                new Cube(){
                                    frontSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        ),
                                    backSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        )
                                }
                            }
                        });
            Index index14 = new Index(
                    14, 
                    new IndexConnections[]{
                    }, 
                    new MeshStructure(){
                            drawCube = new Cube(){
                                frontSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    ),
                                backSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    )
                            },
                            deleteFromCube = new Cube[]{
                                new Cube(){
                                    frontSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        ),
                                    backSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        )
                                }
                            }
                        }
                    );
            Index index15 = new Index(
                    15, 
                    new IndexConnections[]{
                        connections(17,6f)
                    }, 
                    new MeshStructure(){
                            drawCube = new Cube(){
                                frontSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    ),
                                backSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    )
                            },
                            deleteFromCube = new Cube[]{
                                new Cube(){
                                    frontSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        ),
                                    backSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        )
                                }
                            }
                        }
                    );
            Index index16 = new Index(
                    16, 
                    new IndexConnections[]{
                        connections(18,6f)
                    }, 
                    new MeshStructure(){
                            drawCube = new Cube(){
                                frontSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    ),
                                backSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    )
                            },
                            deleteFromCube = new Cube[]{
                                new Cube(){
                                    frontSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        ),
                                    backSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        )
                                }
                            }
                        }
                    );
            Index index17 = new Index(
                    17, 
                    new IndexConnections[]{
                        connections(19,6f)
                    }, 
                    new MeshStructure(){
                            drawCube = new Cube(){
                                frontSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    ),
                                backSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    )
                            },
                            deleteFromCube = new Cube[]{
                                new Cube(){
                                    frontSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        ),
                                    backSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        )
                                }
                            }
                        }
                    );
            Index index18 = new Index(
                    18, 
                    new IndexConnections[]{
                        connections(20,6f)
                    }, 
                    new MeshStructure(){
                            drawCube = new Cube(){
                                frontSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    ),
                                backSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    )
                            },
                            deleteFromCube = new Cube[]{
                                new Cube(){
                                    frontSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        ),
                                    backSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        )
                                }
                            }
                        }
                    );
            Index index19 = new Index(
                    19, 
                    new IndexConnections[]{
                        connections(37,0),
                        connections(38,0),
                        connections(39,0)
                    }, 
                    new MeshStructure(){
                            drawCube = new Cube(){
                                frontSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    ),
                                backSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    )
                            },
                            deleteFromCube = new Cube[]{
                                new Cube(){
                                    frontSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        ),
                                    backSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        )
                                }
                            }
                        }
                    );
            Index index20 = new Index(
                    20, 
                    new IndexConnections[]{
                        connections(40,0),
                        connections(41,0),
                        connections(42,0),
                    }, 
                    new MeshStructure(){
                            drawCube = new Cube(){
                                frontSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    ),
                                backSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    )
                            },
                            deleteFromCube = new Cube[]{
                                new Cube(){
                                    frontSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        ),
                                    backSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        )
                                }
                            }
                        }
                    );
            Index index21 = new Index(
                    21, 
                    new IndexConnections[]{
                        connections(22,2f)
                    }, 
                    new MeshStructure(){
                            drawCube = new Cube(){
                                frontSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    ),
                                backSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    )
                            },
                            deleteFromCube = new Cube[]{
                                new Cube(){
                                    frontSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        ),
                                    backSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        )
                                }
                            }
                        }
                    );
            Index index22 = new Index(
                    22, new IndexConnections[]{
                    }, 
                    new MeshStructure(){
                            drawCube = new Cube(){
                                frontSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    ),
                                backSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    )
                            },
                            deleteFromCube = new Cube[]{
                                new Cube(){
                                    frontSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        ),
                                    backSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        )
                                }
                            }
                        }
                    );  
            Index index23 = new Index(
                    23, 
                    new IndexConnections[]{
                        connections(30,2f)
                    }, 
                    new MeshStructure(){
                            drawCube = new Cube(){
                                frontSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    ),
                                backSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    )
                            },
                            deleteFromCube = new Cube[]{
                                new Cube(){
                                    frontSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        ),
                                    backSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        )
                                }
                            }
                        }
                    );
            Index index24 = new Index(
                    24, 
                    new IndexConnections[]{
                        connections(25,2f),
                    }, 
                    new MeshStructure(){
                            drawCube = new Cube(){
                                frontSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    ),
                                backSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    )
                            },
                            deleteFromCube = new Cube[]{
                                new Cube(){
                                    frontSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        ),
                                    backSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        )
                                }
                            }
                        }
                    );
            Index index25 = new Index(
                    25, 
                    new IndexConnections[]{
                    }, 
                    new MeshStructure(){
                            drawCube = new Cube(){
                                frontSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    ),
                                backSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    )
                            },
                            deleteFromCube = new Cube[]{
                                new Cube(){
                                    frontSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        ),
                                    backSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        )
                                }
                            }
                        }
                    );
            Index index26 = new Index(
                    26, new IndexConnections[]{
                        connections(27,2f),
                    }, 
                    new MeshStructure(){
                            drawCube = new Cube(){
                                frontSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    ),
                                backSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    )
                            },
                            deleteFromCube = new Cube[]{
                                new Cube(){
                                    frontSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        ),
                                    backSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        )
                                }
                            }
                        }
                    );
            Index index27 = new Index(
                    27, 
                    new IndexConnections[]{
                    },
                    new MeshStructure(){
                            drawCube = new Cube(){
                                frontSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    ),
                                backSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    )
                            },
                            deleteFromCube = new Cube[]{
                                new Cube(){
                                    frontSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        ),
                                    backSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        )
                                }
                            }
                        }
                    );
            Index index28 = new Index(
                    28, 
                    new IndexConnections[]{
                        connections(29,2f),
                    }, 
                    new MeshStructure(){
                            drawCube = new Cube(){
                                frontSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    ),
                                backSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    )
                            },
                            deleteFromCube = new Cube[]{
                                new Cube(){
                                    frontSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        ),
                                    backSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        )
                                }
                            }
                        }
                    );
            Index index29 = new Index(
                    29, 
                    new IndexConnections[]{
                    }, 
                    new MeshStructure(){
                            drawCube = new Cube(){
                                frontSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    ),
                                backSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    )
                            },
                            deleteFromCube = new Cube[]{
                                new Cube(){
                                    frontSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        ),
                                    backSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        )
                                }
                            }
                        }
                    );
            Index index30 = new Index(
                    30, 
                    new IndexConnections[]{
                    }, 
                    new MeshStructure(){
                            drawCube = new Cube(){
                                frontSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    ),
                                backSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    )
                            },
                            deleteFromCube = new Cube[]{
                                new Cube(){
                                    frontSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        ),
                                    backSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        )
                                }
                            }
                        }
                    );
            Index index31 = new Index(
                    31, 
                    new IndexConnections[]{
                        connections(32,2f),
                    }, 
                    new MeshStructure(){
                            drawCube = new Cube(){
                                frontSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    ),
                                backSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    )
                            },
                            deleteFromCube = new Cube[]{
                                new Cube(){
                                    frontSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        ),
                                    backSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        )
                                }
                            }
                        }
                    );
            Index index32 = new Index(
                    32, 
                    new IndexConnections[]{
                    }, 
                    new MeshStructure(){
                            drawCube = new Cube(){
                                frontSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    ),
                                backSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    )
                            },
                            deleteFromCube = new Cube[]{
                                new Cube(){
                                    frontSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        ),
                                    backSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        )
                                }
                            }
                        }
                    );
            Index index33 = new Index(
                    33, 
                    new IndexConnections[]{
                        connections(15,6f)
                    }, 
                    new MeshStructure(){
                            drawCube = new Cube(){
                                frontSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    ),
                                backSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    )
                            },
                            deleteFromCube = new Cube[]{
                                new Cube(){
                                    frontSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        ),
                                    backSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        )
                                }
                            }
                        }
                    );
            Index index34 = new Index(
                    34, 
                    new IndexConnections[]{
                        connections(16,6f)
                    }, 
                    new MeshStructure(){
                            drawCube = new Cube(){
                                frontSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    ),
                                backSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    )
                            },
                            deleteFromCube = new Cube[]{
                                new Cube(){
                                    frontSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        ),
                                    backSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        )
                                }
                            }
                        }
                    );
            Index index35 = new Index(
                    35, 
                    new IndexConnections[]{
                        connections(7,6f)
                    }, 
                    new MeshStructure(){
                            drawCube = new Cube(){
                                frontSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    ),
                                backSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    )
                            },
                            deleteFromCube = new Cube[]{
                                new Cube(){
                                    frontSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        ),
                                    backSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        )
                                }
                            }
                        }
                    );
            Index index36 = new Index(
                    36, 
                    new IndexConnections[]{
                        connections(8,6f)
                    }, 
                    new MeshStructure(){
                            drawCube = new Cube(){
                                frontSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    ),
                                backSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    )
                            },
                            deleteFromCube = new Cube[]{
                                new Cube(){
                                    frontSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        ),
                                    backSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        )
                                }
                            }
                        }
                    );
           Index index37 = new Index(
                    37, 
                    new IndexConnections[]{
                        connections(21,2)
                    }, 
                    new MeshStructure(){
                            drawCube = new Cube(){
                                frontSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    ),
                                backSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    )
                            },
                            deleteFromCube = new Cube[]{
                                new Cube(){
                                    frontSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        ),
                                    backSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        )
                                }
                            }
                        }
                    );
            Index index38 = new Index(
                    38, 
                    new IndexConnections[]{
                        connections(23,2)
                    }, 
                    new MeshStructure(){
                            drawCube = new Cube(){
                                frontSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    ),
                                backSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    )
                            },
                            deleteFromCube = new Cube[]{
                                new Cube(){
                                    frontSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        ),
                                    backSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        )
                                }
                            }
                        }
                    );
            Index index39 = new Index(
                    39, 
                    new IndexConnections[]{
                        connections(31,2)
                    }, 
                    new MeshStructure(){
                            drawCube = new Cube(){
                                frontSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    ),
                                backSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    )
                            },
                            deleteFromCube = new Cube[]{
                                new Cube(){
                                    frontSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        ),
                                    backSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        )
                                }
                            }
                        }
                    );
            Index index40 = new Index(
                    40, 
                    new IndexConnections[]{
                        connections(24,2)
                    }, 
                    new MeshStructure(){
                            drawCube = new Cube(){
                                frontSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    ),
                                backSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    )
                            },
                            deleteFromCube = new Cube[]{
                                new Cube(){
                                    frontSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        ),
                                    backSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        )
                                }
                            }
                        }
                    );
            Index index41 = new Index(
                    41, 
                    new IndexConnections[]{
                        connections(26,2)
                    }, 
                    new MeshStructure(){
                            drawCube = new Cube(){
                                frontSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    ),
                                backSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    )
                            },
                            deleteFromCube = new Cube[]{
                                new Cube(){
                                    frontSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        ),
                                    backSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        )
                                }
                            }
                        }
                    );
            Index index42 = new Index(
                    42, 
                    new IndexConnections[]{
                        connections(28,2)
                    }, 
                    new MeshStructure(){
                            drawCube = new Cube(){
                                frontSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    ),
                                backSquare = distanceFromCenter(
                                    corner(0,0,0),corner(0,0,0),
                                    corner(0,0,0),corner(0,0,0)
                                    )
                            },
                            deleteFromCube = new Cube[]{
                                new Cube(){
                                    frontSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        ),
                                    backSquare = distanceFromCenter(
                                        corner(0,0,0),corner(0,0,0),
                                        corner(0,0,0),corner(0,0,0)
                                        )
                                }
                            }
                        }
                    );
            jointList = new List<Index>{
                index0,index1,index26,index27,index28,index29,
                index30,index31,index32,index2,index3,index4,
                index5,index6,index17,index18,index19,
                index20,index21,index22,index7,index8,index9,
                index10,index11,index12,index13,index14,
                index15,index16,index23,index24,
                index25,index33,index34,
                index35,index36,index37,index38,index39,
                index40,index41,index42
            };
            sortList();
        }
    }

    void Start(){
        // int con = 0;
        // int con2 = 5;
        // while(con != con2){
        //     con++;
        //     print(con);
        // }
        body = new HumanBody();
        body.fun();
        Vector3 startPoint = new Vector3(20,50,20);
        body.jointHierarchy(startPoint);
        body.globalPoint(startPoint);

        //Right Arm
        body.rotateLocal(90f,33,3,false);
        body.rotateLocal(-45f,15,3,false);
        body.rotateLocal(-45f,17,3,false);
        //Right Hand
        body.rotateLocal(75f,37,2,false);
        body.rotateLocal(-60f,37,1,false);
        body.rotateLocal(25f,21,1,false);

        body.rotateLocal(-60f,38,1,false);
        body.rotateLocal(25f,23,1,false);

        body.rotateLocal(-75f,39,2,false);
        body.rotateLocal(-60f,39,1,false);
        body.rotateLocal(25f,31,1,false);
        //Right Leg
        body.rotateLocal(60f,35,3,false);
        body.rotateLocal(-60f,7,3,false);
        body.rotateLocal(-90f,11,1,false);

        //Left Arm
        body.rotateLocal(-90f,34,3,false);
        body.rotateLocal(45f,16,3,false);
        body.rotateLocal(45f,18,3,false);
        //Right Hand
        body.rotateLocal(-75f,40,2,false);
        body.rotateLocal(-60f,40,1,false);
        body.rotateLocal(25f,28,1,false);

        body.rotateLocal(-60f,41,1,false);
        body.rotateLocal(25f,26,1,false);

        body.rotateLocal(75f,42,2,false);
        body.rotateLocal(-60f,42,1,false);
        body.rotateLocal(25f,24,1,false);
        //Left Leg
        body.rotateLocal(-60f,36,3,false);
        body.rotateLocal(60f,8,3,false);
        body.rotateLocal(-90f,12,1,false);

        bod = new Vector3[]{
            new Vector3(25,11,-70)
        };
        // body.rotateLocal(-90f,0,1,false);
        draw(0);
        // Vector3[] meshBody = new List<Vector3>(body.meshGeneration(0).Values).ToArray();
        // meshBody = WorldBuilder.Movement.rotateObject(-90f,body.local[0],meshBody,body.local[1]);
        // WorldBuilder.BitArrayManipulator.createOrDeleteObject(meshBody,true,1);
        body.meshGeneration(0);
    }

    float time = 0;
    bool once = true;
    Vector3[] bod;
    void Update(){
        // if (once){
        //     renumberIndex(
        //         jointList
        //         );
        // }
        // time += Time.deltaTime;
        // if (time >0.01f){
        //     draw(0);
        //     time = 0f;
        // }
    }
    public void draw(int choice){

            body.drawLocal(false);
            // body.moveGlobal(-1f,1);
            // body.globalPoint(1f,2);
            body.drawLocal(true);

    }
}
